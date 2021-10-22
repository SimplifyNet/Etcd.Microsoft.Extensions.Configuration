using System;

namespace Etcd.Microsoft.Extensions.Configuration
{
	/// <summary>
	/// Provides etcd related exceptions
	/// </summary>
	/// <seealso cref="Exception" />
	public class EtcdException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EtcdException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
		public EtcdException(string message, Exception? innerException = null) : base(message, innerException)
		{
		}
	}
}