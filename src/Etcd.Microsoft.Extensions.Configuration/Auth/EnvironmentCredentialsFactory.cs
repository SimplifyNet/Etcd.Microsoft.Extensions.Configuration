namespace Etcd.Microsoft.Extensions.Configuration.Auth;

/// <summary>
/// Provides a factory for creating environment-based credentials for etcd.
/// </summary>
public static class EnvironmentCredentialsFactory
{
	/// <summary>
	/// Tries to create credentials from environment variables.
	/// </summary>
	/// <returns>Credentials if both username and password are set; otherwise, null.</returns>
	public static ICredentials? TryCreate()
	{
		var userName = EtcdApplicationEnvironment.UserName;
		var password = EtcdApplicationEnvironment.Password;

		if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
			return null;

		return new Credentials(userName, password);
	}
}