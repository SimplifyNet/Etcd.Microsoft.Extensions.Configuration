namespace Etcd.Microsoft.Extensions.Configuration.Auth
{
	/// <summary>
	/// Represents credentials
	/// </summary>
	public interface ICredentials
	{
		/// <summary>
		/// Gets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		string UserName { get; }

		/// <summary>
		/// Gets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
		string Password { get; }
	}
}