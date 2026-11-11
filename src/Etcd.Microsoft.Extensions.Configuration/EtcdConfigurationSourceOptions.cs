using System;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Client;

namespace Etcd.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides immutable options for <see cref="EtcdConfigurationSource" />.
/// </summary>
public class EtcdConfigurationSourceOptions
{
	/// <summary>
	/// Initializes a new instance of the <see cref="EtcdConfigurationSourceOptions" /> class.
	/// </summary>
	/// <param name="clientFactory">The client factory.</param>
	/// <param name="credentials">The credentials.</param>
	/// <param name="keyPrefix">The key prefix.</param>
	/// <param name="keyPrefixSeparator">The key prefix separator.</param>
	/// <param name="enableWatch">if set to <c>true</c> the keys watch will be enabled.</param>
	/// <param name="unwatchOnDispose">if set to <c>true</c> the watching keys will be unwatched on dispose.</param>
	/// <exception cref="ArgumentNullException">clientFactory
	/// or
	/// credentials</exception>
	public EtcdConfigurationSourceOptions(IEtcdClientFactory clientFactory,
		ICredentials credentials,
		string? keyPrefix = null,
		string keyPrefixSeparator = EtcdConfigurationProvider.DefaultKeyPrefixSeparator,
		bool enableWatch = true,
		bool unwatchOnDispose = true)
	{
		ArgumentNullException.ThrowIfNull(clientFactory);
		ArgumentNullException.ThrowIfNull(credentials);

		ClientFactory = clientFactory;
		Credentials = credentials;
		KeyPrefix = keyPrefix;
		KeyPrefixSeparator = keyPrefixSeparator;
		EnableWatch = enableWatch;
		UnwatchOnDispose = unwatchOnDispose;
	}

	/// <summary>
	/// Gets the client factory.
	/// </summary>
	public IEtcdClientFactory ClientFactory { get; }

	/// <summary>
	/// Gets the credentials.
	/// </summary>
	public ICredentials Credentials { get; }

	/// <summary>
	/// Gets the key prefix.
	/// </summary>
	public string? KeyPrefix { get; }

	/// <summary>
	/// Gets the key prefix separator.
	/// </summary>
	public string KeyPrefixSeparator { get; }

	/// <summary>
	/// Gets a value indicating whether the keys watch is enabled.
	/// </summary>
	public bool EnableWatch { get; }

	/// <summary>
	/// Gets a value indicating whether the watching keys will be unwatched on dispose.
	/// </summary>
	public bool UnwatchOnDispose { get; }
}
