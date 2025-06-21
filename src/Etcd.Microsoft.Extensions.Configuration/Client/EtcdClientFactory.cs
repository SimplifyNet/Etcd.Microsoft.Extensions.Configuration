using System;
using dotnet_etcd;
using dotnet_etcd.interfaces;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Grpc.Core;

namespace Etcd.Microsoft.Extensions.Configuration.Client;

/// <summary>
/// Provides etcd client factory
/// </summary>
public class EtcdClientFactory : IEtcdClientFactory
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EtcdClientFactory" /> class.
	/// </summary>
	/// <param name="settings">The settings.</param>
	/// <exception cref="ArgumentNullException">settings</exception>
	public EtcdClientFactory(IEtcdSettings? settings = null)
	{
		var environmentSetting = EnvironmentSettingsFactory.Create();

		Settings = new EtcdSettings(settings?.ConnectionString ?? environmentSetting.ConnectionString);
	}

	/// <summary>
	/// Gets the settings.
	/// </summary>
	/// <value>
	/// The settings.
	/// </value>
	public IEtcdSettings Settings { get; }

	/// <summary>
	/// Creates the etcd client instance.
	/// </summary>
	/// <returns></returns>
	public IEtcdClient Create()
	{
		if (string.IsNullOrEmpty(Settings.ConnectionString))
			throw new EtcdConfigurationException("Connection string is missing, should be passed in AddEtcd parameters or set in environment variables.");

		return new EtcdClient(Settings.ConnectionString, configureChannelOptions: (options =>
		{
			options.Credentials = Settings.ConnectionString!.StartsWith("https")
				? ChannelCredentials.SecureSsl
				: ChannelCredentials.Insecure;
		}));
	}
}