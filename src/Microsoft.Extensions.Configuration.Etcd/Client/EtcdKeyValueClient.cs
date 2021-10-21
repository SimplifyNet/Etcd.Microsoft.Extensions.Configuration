using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.Etcd.Auth;
using Microsoft.Extensions.Configuration.Etcd.Watch;
using Authpb;
using dotnet_etcd.interfaces;
using Etcdserverpb;
using Google.Protobuf;
using Grpc.Core;

using Convert = Microsoft.Extensions.Configuration.Etcd.Util.Convert;

namespace Microsoft.Extensions.Configuration.Etcd.Client
{
	/// <summary>
	/// Provides etcd key-value client with additional functionality
	/// </summary>
	public class EtcdKeyValueClient : IEtcdKeyValueClient
	{
		private readonly IEtcdClient _client;
		private readonly bool _enableWatch;
		private readonly bool _unwatchOnDispose;

		private readonly IList<long> _watchIDs = new List<long>();
		private readonly object _locker = new();

		private string? _userName;
		private string? _token;

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
		public EtcdKeyValueClient(IEtcdClientFactory clientFactory, ICredentials credentials, bool enableWatch = true, bool unwatchOnDispose = true)
		{
			if (clientFactory == null) throw new ArgumentNullException(nameof(clientFactory));
			if (credentials == null) throw new ArgumentNullException(nameof(credentials));

			_client = clientFactory.Create();
			_enableWatch = enableWatch;
			_unwatchOnDispose = unwatchOnDispose;

			Authenticate(credentials);
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="EtcdKeyValueClient"/> class.
		/// </summary>
		~EtcdKeyValueClient() => Dispose();

		/// <summary>
		/// Occurs when watch callback is received.
		/// </summary>
		public event WatchHandler? WatchCallback;

		/// <summary>
		/// Gets all keys available to user.
		/// </summary>
		/// <returns></returns>
		public IDictionary<string, string> GetAllKeys()
		{
			CheckIsAuthenticated();

			try
			{
				var roles = GetRoles();
				var permissions = new List<Permission>();
				var keys = new List<Mvccpb.KeyValue>();

				foreach (var role in roles)
					permissions.AddRange(GetPermissions(role));

				foreach (var permission in permissions)
					keys.AddRange(GetKeys(permission.Key.ToStringUtf8()));

				if (_enableWatch)
					foreach (var item in keys)
						Watch(item.Key.ToStringUtf8());

				return Convert.ToDictionary(keys);
			}
			catch (RpcException e)
			{
				throw new EtcdException($"Error reading all keys from etcd provider", e);
			}
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns></returns>
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
			if (_unwatchOnDispose)
				StopWatchAll();

			_client.Dispose();
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

		private IEnumerable<Mvccpb.KeyValue> GetKeys(string prefixKey) => _client.GetRange(prefixKey, GetMetadata()).Kvs;

		private IEnumerable<string> GetRoles() =>
			_client.UserGet(new AuthUserGetRequest
			{
				Name = _userName
			}, GetMetadata()).Roles;

		private IEnumerable<Permission> GetPermissions(string role) =>
			_client.RoleGet(new AuthRoleGetRequest
			{
				Role = role
			}, GetMetadata()).Perm;

		private Metadata GetMetadata() =>
			new()
			{
				new("token", _token)
			};

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

		private Task StopWatchAsync(long watchID)
		{
			return _client.Watch(new WatchRequest
			{
				CancelRequest = new WatchCancelRequest
				{
					WatchId = watchID
				}
			}, OnWatchCallback, GetMetadata());
		}

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
			_client.Watch(new WatchRequest
			{
				CreateRequest = new WatchCreateRequest
				{
					Key = ByteString.CopyFromUtf8(key)
				}
			}, OnWatchCallback, GetMetadata());
	}
}