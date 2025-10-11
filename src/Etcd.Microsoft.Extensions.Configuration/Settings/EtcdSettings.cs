namespace Etcd.Microsoft.Extensions.Configuration.Settings;

/// <summary>
/// Provides etcd settings
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConfigurationBasedEtcdSettings" /> class.
/// </remarks>
/// <returns></returns>
/// <exception cref="EtcdConfigurationException">`{ConfigurationSectionName}` configuration section is missing
/// or
/// '{nameof(ConnectionString)}' is missing in configuration</exception>
public class EtcdSettings(string? connectionString) : IEtcdSettings
{
	/// <summary>
	/// Gets the connection string.
	/// </summary>
	/// <value>
	/// The connection string.
	/// </value>
	public string? ConnectionString { get; } = connectionString;
}