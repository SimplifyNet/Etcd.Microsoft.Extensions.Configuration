namespace Etcd.Microsoft.Extensions.Configuration.Settings;

/// <summary>
/// Provides etcd settings from environment
/// </summary>
public class EnvironmentSettingsFactory
{
	/// <summary>
	/// Creates this instance.
	/// </summary>
	/// <returns></returns>
	public static IEtcdSettings Create() => new EtcdSettings(EtcdApplicationEnvironment.ConnectionString);
}