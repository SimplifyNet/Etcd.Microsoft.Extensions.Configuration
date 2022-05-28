using System;
using System.Collections.Generic;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Etcd.Microsoft.Extensions.Configuration.Watch;

namespace Etcd.Microsoft.Extensions.Configuration.Client;

/// <summary>
/// Represents etcd key-value client
/// </summary>
public interface IEtcdKeyValueClient : IDisposable
{
	/// <summary>
	/// Occurs when watch callback is received.
	/// </summary>
	event WatchHandler? WatchCallback;

	/// <summary>
	/// Gets the settings.
	/// </summary>
	/// <value>
	/// The settings.
	/// </value>
	IEtcdSettings Settings { get; }

	/// <summary>
	/// Gets all keys available to user.
	/// </summary>
	/// <returns></returns>
	IDictionary<string, string> GetAllKeys();

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns></returns>
	string? GetValue(string key);
}