using System;

namespace Microsoft.Extensions.Configuration.Etcd
{
	/// <summary>
	/// Provides etcd configuration related exceptions
	/// </summary>
	/// <seealso cref="Exception" />
	public class EtcdConfigurationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EtcdConfigurationException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public EtcdConfigurationException(string message) : base(message)
		{
		}
	}
}