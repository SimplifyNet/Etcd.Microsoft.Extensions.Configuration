namespace Microsoft.Extensions.Configuration.Etcd.Settings
{
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
		public EtcdSettings(string connectionString, string? certificateData = null)
		{
			ConnectionString = connectionString;
			CertificateData = certificateData;
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
}