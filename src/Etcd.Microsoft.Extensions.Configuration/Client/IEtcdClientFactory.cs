using dotnet_etcd.interfaces;
using Etcd.Microsoft.Extensions.Configuration.Settings;

namespace Etcd.Microsoft.Extensions.Configuration.Client;

/// <summary>
/// Represents etcd client factory
/// </summary>
public interface IEtcdClientFactory
{
	/// <summary>
	/// Gets the settings.
	/// </summary>
	/// <value>
	/// The settings.
	/// </value>
	IEtcdSettings Settings { get; }

	/// <summary>
	/// Creates the etcd client instance.
	/// </summary>
	/// <returns></returns>
	IEtcdClient Create();
}