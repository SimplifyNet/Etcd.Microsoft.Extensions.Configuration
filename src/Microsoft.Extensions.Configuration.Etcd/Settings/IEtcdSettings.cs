namespace Microsoft.Extensions.Configuration.Etcd.Settings
{
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
		string ConnectionString { get; }

		/// <summary>
		/// Gets the certificate data.
		/// </summary>
		/// <value>
		/// The certificate data.
		/// </value>
		string? CertificateData { get; }
	}
}