using dotnet_etcd.interfaces;

namespace Etcd.Microsoft.Extensions.Configuration.Client
{
	/// <summary>
	/// Represents etcd client factory
	/// </summary>
	public interface IEtcdClientFactory
	{
		/// <summary>
		/// Creates the etcd client instance.
		/// </summary>
		/// <returns></returns>
		IEtcdClient Create();
	}
}