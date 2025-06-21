using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authpb;
using dotnet_etcd.interfaces;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Etcd.Microsoft.Extensions.Configuration.Watch;
using Etcdserverpb;
using Google.Protobuf.Collections;
using Grpc.Core;

using Convert = Etcd.Microsoft.Extensions.Configuration.Util.Convert;

namespace Etcd.Microsoft.Extensions.Configuration.Client;

/// <summary>
/// Provides etcd key-value client with additional functionality
/// </summary>
public class EtcdKeyValueClient : IEtcdKeyValueClient
{
	private readonly IEtcdClient _client;
	private readonly IEtcdClientFactory _clientFactory;
	private readonly bool _enableWatch;
	private readonly bool _unwatchOnDispose;

	private readonly IList<long> _watchIDs = [];
	private readonly object _locker = new();

	private string? _userName;
	private string? _token;

	private bool _isDisposed;

	/// <summary>
	/// Initializes a new instance of the <see cref="EtcdKeyValueClient" /> class.
	/// </summary>
	/// <param name="clientFactory">The client factory.</param>
	/// <param name="credentials">The credentials.</param>
	/// <param name="enableWatch">if set to <c>true</c> the keys watch will be enabled.</param>
	/// <param name="unwatchOnDispose">if set to <c>true</c> the watching keys will be unwatched on dispose .</param>
	/// <exception cref="ArgumentNullException">clientFactory
	/// or
	/// credentials</exception>
	// ReSharper disable once TooManyDependencies
	public EtcdKeyValueClient(IEtcdClientFactory clientFactory, ICredentials credentials, bool enableWatch = true, bool unwatchOnDispose = true)
	{
		ArgumentNullException.ThrowIfNull(clientFactory);
		ArgumentNullException.ThrowIfNull(credentials);

		_client = clientFactory.Create();
		_clientFactory = clientFactory;
		_enableWatch = enableWatch;
		_unwatchOnDispose = unwatchOnDispose;

		Authenticate(credentials);
	}

	/// <summary>
	/// Finalizes an instance of the <see cref="EtcdKeyValueClient"/> class.
	/// </summary>
	~EtcdKeyValueClient() => Dispose(false);

	/// <summary>
	/// Occurs when watch callback is received.
	/// </summary>
	public event WatchHandler? WatchCallback;

	/// <summary>
	/// Gets the settings.
	/// </summary>
	/// <value>
	/// The settings.
	/// </value>
	public IEtcdSettings Settings => _clientFactory.Settings;

	/// <summary>
	/// Gets all keys available to user.
	/// </summary>
	public IDictionary<string, string?> GetAllKeys()
	{
		CheckIsAuthenticated();

		try
		{
			var roles = GetRoles();
			var permissions = GetAllPermissions(roles);
			var keys = GetAllKeys(permissions);

			if (_enableWatch)
				WatchAll(keys);

			return Convert.ToDictionary(keys);
		}
		catch (RpcException e)
		{
			throw new EtcdException("Error reading all keys from etcd provider", e);
		}
	}

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <param name="key">The key.</param>
	public string? GetValue(string key)
	{
		CheckIsAuthenticated();

		try
		{
			return _client.GetVal(key, GetMetadata());
		}
		catch (RpcException e)
		{
			throw new EtcdException($"Error reading value for key '{key}' from etcd provider", e);
		}
	}

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Releases unmanaged and - optionally - managed resources.
	/// </summary>
	/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
	// ReSharper disable once FlagArgument
	protected virtual void Dispose(bool disposing)
	{
		if (!disposing || _isDisposed)
			return;

		if (_unwatchOnDispose)
			StopWatchAll();

		_client.Dispose();

		_isDisposed = true;
	}

	/// <summary>
	/// Authenticates the client.
	/// </summary>
	/// <param name="credentials">The credentials.</param>
	private void Authenticate(ICredentials credentials)
	{
		var result = _client.Authenticate(new AuthenticateRequest
		{
			Name = credentials.UserName,
			Password = credentials.Password
		});

		_token = result.Token;
		_userName = credentials.UserName;
	}

	private RepeatedField<Mvccpb.KeyValue> GetKeys(string prefixKey)
	{
		// Fix to correct read all keys
		if (prefixKey == "\0")
			prefixKey = "";

		return _client.GetRange(prefixKey, GetMetadata()).Kvs;
	}

	private RepeatedField<string> GetRoles() =>
		_client.UserGet(new AuthUserGetRequest
		{
			Name = _userName
		}, GetMetadata()).Roles;

	private List<Permission> GetAllPermissions(RepeatedField<string> roles)
	{
		var permissions = new List<Permission>();

		foreach (var role in roles)
			permissions.AddRange(GetPermissions(role));

		return permissions;
	}

	private List<Mvccpb.KeyValue> GetAllKeys(List<Permission> permissions)
	{
		var keys = new List<Mvccpb.KeyValue>();

		foreach (var permission in permissions)
			keys.AddRange(GetKeys(permission.Key.ToStringUtf8()));

		return keys;
	}

	private void WatchAll(IList<Mvccpb.KeyValue> keys)
	{
		foreach (var item in keys)
			Watch(item.Key.ToStringUtf8());
	}

	private RepeatedField<Permission> GetPermissions(string role) =>
		_client.RoleGet(new AuthRoleGetRequest
		{
			Role = role
		}, GetMetadata()).Perm;

	private Metadata GetMetadata() =>
		[new Metadata.Entry("token", _token!)];

	private void CheckIsAuthenticated()
	{
		if (_token == null || _userName == null)
			throw new InvalidOperationException("Etcd client is not authenticated");
	}

	private void OnWatchCallback(WatchResponse response)
	{
		if (_unwatchOnDispose)
			lock (_locker)
			{
				if (!_watchIDs.Contains(response.WatchId))
					_watchIDs.Add(response.WatchId);
			}

		WatchCallback?.Invoke(response.Events.Select(x =>
			new WatchEvent(Convert.ToEventType(x.Type),
				x.Kv.Key.ToStringUtf8(),
				x.Kv.Value.ToStringUtf8())));
	}

	private Task<long> StopWatchAsync(long watchID) =>
		_client.WatchAsync(new WatchRequest
		{
			CancelRequest = new WatchCancelRequest
			{
				WatchId = watchID
			}
		}, OnWatchCallback, GetMetadata());

	private void StopWatchAll()
	{
		lock (_locker)
		{
			foreach (var item in _watchIDs.ToList())
			{
				try
				{
					StopWatchAsync(item).Wait(200);
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}

				_watchIDs.Remove(item);
			}
		}
	}

	private void Watch(string key) =>
		_client.Watch(key, OnWatchCallback, GetMetadata());
}