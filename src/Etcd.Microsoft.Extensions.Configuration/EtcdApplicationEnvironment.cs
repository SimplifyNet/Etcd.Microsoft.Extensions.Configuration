using System;

namespace Etcd.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides etcd application environment
/// </summary>
public static class EtcdApplicationEnvironment
{
	/// <summary>
	/// The connection string environment variable name
	/// </summary>
	public const string ConnectionStringEnvironmentVariableName = "ETCD_CLIENT_CONNECTION_STRING";

	private static string? _connectionString;

	/// <summary>
	/// Gets or sets the connection string.
	/// </summary>
	/// <value>
	/// The connection string.
	/// </value>
	/// <exception cref="System.ArgumentNullException">value</exception>
	public static string? ConnectionString
	{
		get
		{
			return _connectionString ??= Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariableName);
		}
		set => _connectionString = value ?? throw new ArgumentNullException(nameof(value));
	}
}