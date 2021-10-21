using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration.Etcd.Watch;

namespace Microsoft.Extensions.Configuration.Etcd.Client
{
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
}