using System;

namespace Etcd.Microsoft.Extensions.Configuration.Auth;

/// <summary>
/// Provides credentials
/// </summary>
/// <seealso cref="ICredentials" />
public class Credentials : ICredentials
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Credentials"/> class.
	/// </summary>
	/// <param name="userName">Name of the user.</param>
	/// <param name="password">The password.</param>
	/// <exception cref="ArgumentException">
	/// Value cannot be null or empty. - userName
	/// or
	/// Value cannot be null or empty. - password
	/// </exception>
	public Credentials(string userName, string password)
	{
		if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", nameof(userName));
		if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", nameof(password));

		UserName = userName;
		Password = password;
	}

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
}