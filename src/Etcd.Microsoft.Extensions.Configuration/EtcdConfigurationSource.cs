using System;
using Etcd.Microsoft.Extensions.Configuration.Client;
using Microsoft.Extensions.Configuration;

namespace Etcd.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides etcd based IConfigurationSource implementation
/// </summary>
/// <seealso cref="IConfigurationSource" />
public class EtcdConfigurationSource : IConfigurationSource
{
	private readonly EtcdConfigurationSourceOptions _options;

	/// <summary>
	/// Initializes a new instance of the <see cref="EtcdConfigurationSource" /> class.
	/// </summary>
	/// <param name="options">The source options.</param>
	/// <exception cref="ArgumentNullException">options</exception>
	public EtcdConfigurationSource(EtcdConfigurationSourceOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);

		_options = options;
	}

	/// <summary>
	/// Gets the key prefix.
	/// </summary>
	/// <value>
	/// The key prefix.
	/// </value>
	public string? KeyPrefix => _options.KeyPrefix;

	/// <summary>
	/// Gets the key prefix separator.
	/// </summary>
	/// <value>
	/// The key prefix separator.
	/// </value>
	public string? KeyPrefixSeparator => _options.KeyPrefixSeparator;

	/// <summary>
	/// Builds the <see cref="T:IConfigurationProvider" /> for this source.
	/// </summary>
	/// <param name="builder">The <see cref="T:IConfigurationBuilder" />.</param>
	/// <returns>
	/// An <see cref="T:IConfigurationProvider" />
	/// </returns>
	public IConfigurationProvider Build(IConfigurationBuilder builder) =>
		new EtcdConfigurationProvider(
			new EtcdKeyValueClient(
				_options.ClientFactory,
				_options.Credentials,
				_options.EnableWatch,
				_options.UnwatchOnDispose),
			KeyPrefix,
			KeyPrefixSeparator);

	/// <summary>
	/// Converts to string (return etcd connection representation).
	/// </summary>
	/// <returns>
	/// A <see cref="string" /> that represents this instance.
	/// </returns>
	public override string ToString()
	{
		var applicationInfo = KeyPrefix != null
			? $" {KeyPrefix} -"
			: "";

		return $"Etcd -{applicationInfo} {_options.ClientFactory.Settings.ConnectionString}";
	}
}
