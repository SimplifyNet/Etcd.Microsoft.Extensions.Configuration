using System;
using System.IO;

namespace Etcd.Microsoft.Extensions.Configuration
{
	/// <summary>
	/// Provides etcd application environment
	/// </summary>
	public static class EtcdApplicationEnvironment
	{
		/// <summary>
		/// The connection string environment variable name
		/// </summary>
		public const string ConnectionStringEnvironmentVariableName = "ETCD_CLIENT_CONNECTION_STRING";

		/// <summary>
		/// The CA certificate file environment variable name
		/// </summary>
		public const string CaCertificateEnvironmentVariableName = "ETCD_CLIENT_CA_FILE";

		private static string? _connectionString;
		private static string? _caCertificateFilePath;

		/// <summary>
		/// Gets or sets the etcd CA certificate file path.
		/// </summary>
		/// <value>
		/// The ca certificate file path.
		/// </value>
		/// <exception cref="EtcdException">etcd CA environment variable '{CaCertificateEnvironmentVariableName}' is not found</exception>
		/// <exception cref="ArgumentNullException">value</exception>
		public static string CaCertificateFilePath
		{
			get
			{
				return _caCertificateFilePath ??= Environment.GetEnvironmentVariable(CaCertificateEnvironmentVariableName)
												  ?? throw new EtcdException(
													  $"Etcd CA certificate environment variable '{CaCertificateEnvironmentVariableName}' is not found");
			}
			set => _caCertificateFilePath = value ?? throw new ArgumentNullException(nameof(value));
		}

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

		/// <summary>
		/// Gets the CA certificate data from CA certificate file.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="EtcdException">Etcd CA file is empty</exception>
		public static string GetCaCertificateData() => File.ReadAllText(CaCertificateFilePath) ?? throw new EtcdException("Etcd CA certificate file is empty");
	}
}