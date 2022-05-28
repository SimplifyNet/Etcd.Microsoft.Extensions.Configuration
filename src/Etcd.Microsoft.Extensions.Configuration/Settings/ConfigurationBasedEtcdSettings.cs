using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Etcd.Microsoft.Extensions.Configuration.Settings;

/// <summary>
/// Provides IConfiguration based etcd settings
/// </summary>
public class ConfigurationBasedEtcdSettings : IEtcdSettings
{
	private const string DefaultConfigurationSectionName = "EtcdSettings";

	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationBasedEtcdSettings" /> class.
	/// </summary>
	/// <param name="configuration">The configuration.</param>
	/// <param name="configurationSectionName">Name of the configuration section.</param>
	/// <exception cref="EtcdConfigurationException">
	/// `{ConfigurationSectionName}` configuration section is missing
	/// or
	/// '{nameof(ConnectionString)}' is missing in configuration
	/// </exception>
	/// <exception cref="EtcdConfigurationException">`{ConfigurationSectionName}` configuration section is missing
	/// or
	/// '{nameof(ConnectionString)}' is missing in configuration</exception>
	public ConfigurationBasedEtcdSettings(IConfiguration configuration, string configurationSectionName = DefaultConfigurationSectionName)
	{
		var section = configuration.GetSection(configurationSectionName);

		if (!configuration.GetChildren().Any())
			throw new EtcdConfigurationException($"`{configurationSectionName}` configuration section is missing");

		ConnectionString = section[nameof(ConnectionString)] ?? throw new EtcdConfigurationException($"'{nameof(ConnectionString)}' is missing in configuration");
		CertificateData = section[nameof(CertificateData)];
	}

	/// <summary>
	/// Gets the connection string.
	/// </summary>
	/// <value>
	/// The connection string.
	/// </value>
	public string ConnectionString { get; }

	/// <summary>
	/// Gets the certificate data.
	/// </summary>
	/// <value>
	/// The certificate data.
	/// </value>
	public string? CertificateData { get; }
}