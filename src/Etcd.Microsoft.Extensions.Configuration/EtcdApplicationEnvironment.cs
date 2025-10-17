using System;

namespace Etcd.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides etcd application environment
/// </summary>
public static class EtcdApplicationEnvironment
{
	private static string _connectionStringEnvironmentVariableName = "ETCD_CLIENT_CONNECTION_STRING";
	private static string _userNameEnvironmentVariableName = "ETCD_CLIENT_USER_NAME";
	private static string _passwordEnvironmentVariableName = "ETCD_CLIENT_PASSWORD";
	private static string? _connectionString;

	/// <summary>
	/// The connection string environment variable name
	/// </summary>
	public static string ConnectionStringEnvironmentVariableName
	{
		get => _connectionStringEnvironmentVariableName;
		set
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));

			_connectionStringEnvironmentVariableName = value;
		}
	}

	/// <summary>
	/// The client user name environment variable name
	/// </summary>
	public static string UserNameEnvironmentVariableName
	{
		get => _userNameEnvironmentVariableName;
		set
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));

			_userNameEnvironmentVariableName = value;
		}
	}

	/// <summary>
	/// The client password environment variable name
	/// </summary>
	public static string PasswordEnvironmentVariableName
	{
		get => _passwordEnvironmentVariableName;
		set
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));

			_passwordEnvironmentVariableName = value;
		}
	}

	/// <summary>
	/// Gets or sets the connection string.
	/// </summary>
	/// <value>
	/// The connection string.
	/// </value>
	/// <exception cref="ArgumentNullException">value</exception>
	public static string? ConnectionString
	{
		get => _connectionString ??= Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariableName);
		set
		{
			if (string.IsNullOrEmpty(value))
				throw new ArgumentNullException(nameof(value));

			_connectionString = value;
		}
	}

	/// <summary>
	/// Gets or sets the user name.
	/// </summary>
	/// <value>
	/// The user name.
	/// </value>
	/// <exception cref="ArgumentNullException">value</exception>
	public static string? UserName => Environment.GetEnvironmentVariable(UserNameEnvironmentVariableName);

	/// <summary>
	/// Gets or sets the password.
	/// </summary>
	/// <value>
	/// The password.
	/// </value>
	/// <exception cref="ArgumentNullException">value</exception>
	public static string? Password => Environment.GetEnvironmentVariable(PasswordEnvironmentVariableName);
}