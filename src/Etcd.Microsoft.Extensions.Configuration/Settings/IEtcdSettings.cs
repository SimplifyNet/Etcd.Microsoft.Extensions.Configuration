namespace Etcd.Microsoft.Extensions.Configuration.Settings;

/// <summary>
/// Represents etcd settings
/// </summary>
public interface IEtcdSettings
{
	/// <summary>
	/// Gets the connection string.
	/// </summary>
	/// <value>
	/// The connection string.
	/// </value>
	string? ConnectionString { get; }
}