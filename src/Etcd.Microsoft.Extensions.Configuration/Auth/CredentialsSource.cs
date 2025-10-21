namespace Etcd.Microsoft.Extensions.Configuration.Auth
{
	/// <summary>
	/// Possible sources for credentials.
	/// </summary>
	public enum CredentialsSource
	{
		/// <summary>
		/// Credentials provided via code.
		/// </summary>
		Code = 0,
		/// <summary>
		/// Credentials provided via environment variables.
		/// </summary>
		EnvironmentVariables = 1
	}
}