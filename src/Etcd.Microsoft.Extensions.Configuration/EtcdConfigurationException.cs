using System;

namespace Etcd.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides etcd configuration related exceptions
/// </summary>
/// <seealso cref="Exception" />
/// <remarks>
/// Initializes a new instance of the <see cref="EtcdConfigurationException"/> class.
/// </remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
public class EtcdConfigurationException(string message) : Exception(message);