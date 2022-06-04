namespace Etcd.Microsoft.Extensions.Configuration.Settings;

/// <summary>
/// Provides etcd settings
/// </summary>
public class EtcdSettings : IEtcdSettings
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ConfigurationBasedEtcdSettings" /> class.
	/// </summary>
	/// <returns></returns>
	/// <exception cref="EtcdConfigurationException">`{ConfigurationSectionName}` configuration section is missing
	/// or
	/// '{nameof(ConnectionString)}' is missing in configuration</exception>
	public EtcdSettings(string? connectionString) => ConnectionString = connectionString;

	/// <summary>
	/// Gets the connection string.
	/// </summary>
	/// <value>
	/// The connection string.
	/// </value>
	public string? ConnectionString { get; }
}