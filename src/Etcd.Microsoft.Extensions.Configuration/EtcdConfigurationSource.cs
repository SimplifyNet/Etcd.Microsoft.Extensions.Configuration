using Etcd.Microsoft.Extensions.Configuration.Client;
using Microsoft.Extensions.Configuration;

namespace Etcd.Microsoft.Extensions.Configuration;

/// <summary>
/// Provides etcd based IConfigurationSource implementation
/// </summary>
/// <seealso cref="IConfigurationSource" />
/// <remarks>
/// Initializes a new instance of the <see cref="EtcdConfigurationSource" /> class.
/// </remarks>
/// <param name="client">The key value store.</param>
/// <param name="keyPrefix">The key prefix.</param>
/// <param name="keyPrefixSeparator">The key prefix separator.</param>
public class EtcdConfigurationSource(IEtcdKeyValueClient client,
	string? keyPrefix = null,
	string keyPrefixSeparator = EtcdConfigurationProvider.DefaultKeyPrefixSeparator)
	: IConfigurationSource
{
	/// <summary>
	/// Gets the key prefix.
	/// </summary>
	/// <value>
	/// The key prefix.
	/// </value>
	public string? KeyPrefix { get; } = keyPrefix;

	/// <summary>
	/// Gets the key prefix separator.
	/// </summary>
	/// <value>
	/// The key prefix separator.
	/// </value>
	public string? KeyPrefixSeparator { get; } = keyPrefixSeparator;

	/// <summary>
	/// Builds the <see cref="T:IConfigurationProvider" /> for this source.
	/// </summary>
	/// <param name="builder">The <see cref="T:IConfigurationBuilder" />.</param>
	/// <returns>
	/// An <see cref="T:IConfigurationProvider" />
	/// </returns>
	public IConfigurationProvider Build(IConfigurationBuilder builder) => new EtcdConfigurationProvider(client, KeyPrefix, KeyPrefixSeparator);

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

		return $"Etcd -{applicationInfo} {client.Settings.ConnectionString}";
	}
}