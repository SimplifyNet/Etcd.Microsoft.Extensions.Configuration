using System;

namespace Etcd.Microsoft.Extensions.Configuration.Auth;

/// <summary>
/// Provides credentials.
/// </summary>
/// <seealso cref="ICredentials" />
public class Credentials : ICredentials
{
	private const string DefaultUserNameEnvironmentVariableName = "ETCD_CLIENT_USER_NAME";
	private const string DefaultPasswordEnvironmentVariableName = "ETCD_CLIENT_PASSWORD";

	/// <summary>
	/// Initializes a new instance of the <see cref="Credentials"/> class.
	/// </summary>
	/// <param name="userName">Name of the user.</param>
	/// <param name="password">The password.</param>
	/// <param name="userNameSource">The source of the username.</param>
	/// <param name="passwordSource">The source of the password.</param>
	/// <param name="information">The information about the credentials.</param>
	/// <exception cref="ArgumentException">
	/// Value cannot be null or empty. - userName
	/// or
	/// Value cannot be null or empty. - password
	/// </exception>
	public Credentials(string userName, string password,
		CredentialsSource userNameSource = CredentialsSource.Code,
		CredentialsSource passwordSource = CredentialsSource.Code,
		string? information = null)
	{
		if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", nameof(userName));
		if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", nameof(password));

		UserName = userName;
		Password = password;
		UserNameSource = userNameSource;
		PasswordSource = passwordSource;
		Information = information;
	}

	/// <summary>
	/// Gets the source of the user name.
	/// </summary>
	public CredentialsSource UserNameSource { get; }

	/// <summary>
	/// Gets the source of the password.
	/// </summary>
	public CredentialsSource PasswordSource { get; }

	/// <summary>
	/// Gets the name of the user.
	/// </summary>
	/// <value>
	/// The name of the user.
	/// </value>
	public string UserName { get; }

	/// <summary>
	/// Gets the password.
	/// </summary>
	/// <value>
	/// The password.
	/// </value>
	public string Password { get; }

	/// <summary>
	/// Gets the information about the credentials.
	/// </summary>
	public string? Information { get; private set; }

	/// <summary>
	/// Gets the string representation of the credentials.
	/// </summary>
	override public string ToString() => Information ?? "etcd code based credentials";

	/// <summary>
	/// Creates a new credentials instance overriding values from environment variables if they are exists.
	/// </summary>
	/// <param name="userName">The user name.</param>
	/// <param name="password">The password.</param>
	/// <param name="userNameEnvironmentVariableName">The name of the user name environment variable.</param>
	/// <param name="passwordEnvironmentVariableName">The name of the password environment variable.</param>
	/// <exception cref="EtcdException">The etcd user name or password are not provided via code and not found in the environment variable `{userNameEnvironmentVariableName}`.</exception>
	public static ICredentials WithOverrideFromEnvironmentVariables(
		string userName,
		string password,
		string userNameEnvironmentVariableName = DefaultUserNameEnvironmentVariableName,
		string passwordEnvironmentVariableName = DefaultPasswordEnvironmentVariableName)
	{
		var userNameSource = CredentialsSource.Code;
		var passwordSource = CredentialsSource.Code;

		var environmentUserName = Environment.GetEnvironmentVariable(userNameEnvironmentVariableName);
		var environmentPassword = Environment.GetEnvironmentVariable(passwordEnvironmentVariableName);

		if (!string.IsNullOrEmpty(environmentUserName))
		{
			userName = environmentUserName;

			userNameSource = CredentialsSource.EnvironmentVariables;
		}

		if (string.IsNullOrEmpty(userName))
			throw new EtcdException($"Etcd user name is not provided via code and not found in the environment variable `{userNameEnvironmentVariableName}`.");

		if (!string.IsNullOrEmpty(environmentPassword))
		{
			password = environmentPassword;

			passwordSource = CredentialsSource.EnvironmentVariables;
		}

		if (string.IsNullOrEmpty(password))
			throw new EtcdException($"Etcd password is not provided via code and not found in the environment variable `{passwordEnvironmentVariableName}`.");

		return new Credentials(userName, password, userNameSource, passwordSource,
			FormatInformation(
				userNameSource,
				passwordSource,
				userNameEnvironmentVariableName,
				passwordEnvironmentVariableName));
	}

	/// <summary>
	/// Creates a new credentials instance overriding values from environment variables if they are exists.
	/// </summary>
	/// <param name="userName">The user name.</param>
	/// <param name="password">The password.</param>
	/// <param name="passwordEnvironmentVariableName">The name of the password environment variable.</param>
	/// <exception cref="EtcdException">The etcd user name or password are not provided via code and not found in the environment variable `{userNameEnvironmentVariableName}`.</exception>
	public static ICredentials WithOverrideFromEnvironmentVariables(
		string userName,
		string password,
		string passwordEnvironmentVariableName = DefaultPasswordEnvironmentVariableName)
	{
		var userNameSource = CredentialsSource.Code;
		var passwordSource = CredentialsSource.Code;

		var environmentPassword = Environment.GetEnvironmentVariable(passwordEnvironmentVariableName);

		if (string.IsNullOrEmpty(userName))
			throw new EtcdException($"Etcd user name is not provided.");

		if (!string.IsNullOrEmpty(environmentPassword))
		{
			password = environmentPassword;

			passwordSource = CredentialsSource.EnvironmentVariables;
		}

		if (string.IsNullOrEmpty(password))
			throw new EtcdException($"Etcd password is not provided via code and not found in the environment variable `{passwordEnvironmentVariableName}`.");

		return new Credentials(userName, password, userNameSource, passwordSource,
			FormatInformation(
				userNameSource,
				passwordSource,
				null,
				passwordEnvironmentVariableName));
	}


	/// <summary>
	/// Creates a new credentials instance from environment variables.
	/// </summary>
	/// <param name="userNameEnvironmentVariableName">The name of the user name environment variable.</param>
	/// <param name="passwordEnvironmentVariableName">The name of the password environment variable.</param>
	/// <exception cref="EtcdException">The etcd user name or password are not provided via code and not found in the environment variable `{userNameEnvironmentVariableName}`.</exception>
	public static ICredentials FromEnvironmentVariables(
		string userNameEnvironmentVariableName = DefaultUserNameEnvironmentVariableName,
		string passwordEnvironmentVariableName = DefaultPasswordEnvironmentVariableName)
	{
		var userNameSource = CredentialsSource.EnvironmentVariables;
		var passwordSource = CredentialsSource.EnvironmentVariables;

		var userName = Environment.GetEnvironmentVariable(userNameEnvironmentVariableName);
		var password = Environment.GetEnvironmentVariable(passwordEnvironmentVariableName);

		if (string.IsNullOrEmpty(userName))
			throw new EtcdException($"Etcd user name is not found in the environment variable `{userNameEnvironmentVariableName}`.");

		if (string.IsNullOrEmpty(password))
			throw new EtcdException($"Etcd password is not found in the environment variable `{passwordEnvironmentVariableName}`.");

		return new Credentials(userName, password, userNameSource, passwordSource,
			FormatInformation(
				userNameSource,
				passwordSource,
				userNameEnvironmentVariableName,
				passwordEnvironmentVariableName));
	}

	private static string FormatInformation(
	 	CredentialsSource userNameSource,
	 	CredentialsSource passwordSource,
		string? userNameEnvironmentVariableName = null,
		string? passwordEnvironmentVariableName = null) =>
			FormatUserNameInformation(userNameSource, userNameEnvironmentVariableName) + ", " + FormatPassword(passwordSource, passwordEnvironmentVariableName);

	private static string FormatUserNameInformation(CredentialsSource userNameSource, string? userNameEnvironmentVariableName = null)
	{
		var result = $"etcd user name source: {userNameSource}";

		if (userNameSource == CredentialsSource.EnvironmentVariables)
			result += $"({userNameEnvironmentVariableName})";

		return result;
	}

	private static string FormatPassword(CredentialsSource passwordSource, string? passwordEnvironmentVariableName = null)
	{
		var result = $"etcd password source: {passwordSource}";

		if (passwordSource == CredentialsSource.EnvironmentVariables)
			result += $"({passwordEnvironmentVariableName})";

		return result;
	}
}