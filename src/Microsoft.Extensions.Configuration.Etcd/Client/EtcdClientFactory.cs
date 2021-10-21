using System;
using dotnet_etcd;
using dotnet_etcd.interfaces;
using Microsoft.Extensions.Configuration.Etcd.Settings;

namespace Microsoft.Extensions.Configuration.Etcd.Client
{
	/// <summary>
	/// Provides etcd client factory
	/// </summary>
	public class EtcdClientFactory : IEtcdClientFactory
	{
		private readonly IEtcdSettings? _settings;

		/// <summary>
		/// Initializes a new instance of the <see cref="EtcdClientFactory" /> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		/// <exception cref="ArgumentNullException">settings</exception>
		public EtcdClientFactory(IEtcdSettings? settings = null) => _settings = settings;

		/// <summary>
		/// Creates the etcd client instance.
		/// </summary>
		/// <returns></returns>
		public IEtcdClient Create()
		{
			var connectionString = _settings?.ConnectionString ?? EtcdApplicationEnvironment.ConnectionString;

			if (string.IsNullOrEmpty(connectionString))
				throw new EtcdConfigurationException("Connection string is missing, should be passed in AddEtcd parameters or set in environment variables.");

			return connectionString!.StartsWith("https")
				? new EtcdClient(connectionString, caCert: _settings?.CertificateData
														   ?? EtcdApplicationEnvironment.GetCaCertificateData()
														   ?? throw new EtcdConfigurationException("Certificate data is missing, should be passed in AddEtcd parameters or set in environment variables."))
				: new EtcdClient(connectionString);
		}
	}
}