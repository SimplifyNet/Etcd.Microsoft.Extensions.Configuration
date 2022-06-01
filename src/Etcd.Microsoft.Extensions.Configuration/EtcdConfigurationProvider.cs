using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Etcd.Microsoft.Extensions.Configuration.Client;
using Etcd.Microsoft.Extensions.Configuration.Watch;

namespace Etcd.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides etcd based IConfigurationProvider implementation
/// </summary>
/// <seealso cref="IConfigurationProvider" />
public class EtcdConfigurationProvider : ConfigurationProvider, IDisposable
{
	private readonly IEtcdKeyValueClient _client;
	private readonly string? _keyPrefix;

	private readonly object _locker = new();

	/// <summary>
	/// Initializes a new instance of the <see cref="EtcdConfigurationProvider" /> class.
	/// </summary>
	/// <param name="client">The client.</param>
	/// <param name="keyPrefix">The key prefix.</param>
	public EtcdConfigurationProvider(IEtcdKeyValueClient client, string? keyPrefix = null)
	{
		_client = client;
		_keyPrefix = keyPrefix;

		_client.WatchCallback += OnWatchCallback;
	}

	/// <summary>
	/// Loads configuration values from the source represented by this <see cref="T:Microsoft.Extensions.Configuration.IConfigurationProvider" />.
	/// </summary>
	public override void Load() => Data = _client.GetAllKeys();

	/// <summary>
	/// Attempts to find a value with the given key, returns true if one is found, false otherwise.
	/// </summary>
	/// <param name="key">The key to lookup.</param>
	/// <param name="value">The value found at key if one is found.</param>
	/// <returns>
	/// True if key has a value, false otherwise.
	/// </returns>
	public override bool TryGet(string key, out string value) =>
		base.TryGet(_keyPrefix == null
						? key
						: _keyPrefix + ":" + key
					, out value);

	/// <summary>Returns the list of keys that this provider has.</summary>
	/// <param name="earlierKeys">The earlier keys that other providers contain.</param>
	/// <param name="parentPath">The path for the parent IConfiguration.</param>
	/// <returns>The list of keys for this provider.</returns>
	public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath) =>
		base.GetChildKeys(earlierKeys,
			_keyPrefix != null
				? _keyPrefix + ":" + parentPath
		 		: parentPath);

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose() => _client.Dispose();

	private void OnWatchCallback(IEnumerable<WatchEvent> events)
	{
		lock (_locker)
		{
			foreach (var item in events)
			{
				if (item.Type == EventType.Put)
				{
					if (Data.ContainsKey(item.Key))
						Data[item.Key] = item.Value;
					else
						Data.Add(item.Key, item.Value);
				}
				else
					Data.Remove(item.Key);
			}
		}
	}
}