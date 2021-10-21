using Microsoft.Extensions.Configuration.Etcd.Client;

namespace Microsoft.Extensions.Configuration.Etcd
{
	/// <summary>
	/// Provides etcd based IConfigurationSource implementation
	/// </summary>
	/// <seealso cref="IConfigurationSource" />
	public class EtcdConfigurationSource : IConfigurationSource
	{
		private readonly IEtcdKeyValueClient _client;

		/// <summary>
		/// Initializes a new instance of the <see cref="EtcdConfigurationSource" /> class.
		/// </summary>
		/// <param name="client">The key value store.</param>
		/// <param name="keyPrefix">The key prefix.</param>
		public EtcdConfigurationSource(IEtcdKeyValueClient client, string? keyPrefix = null)
		{
			_client = client;
			KeyPrefix = keyPrefix;
		}

		/// <summary>
		/// Gets the key prefix.
		/// </summary>
		/// <value>
		/// The key prefix.
		/// </value>
		public string? KeyPrefix { get; }

		/// <summary>
		/// Builds the <see cref="T:IConfigurationProvider" /> for this source.
		/// </summary>
		/// <param name="builder">The <see cref="T:IConfigurationBuilder" />.</param>
		/// <returns>
		/// An <see cref="T:IConfigurationProvider" />
		/// </returns>
		public IConfigurationProvider Build(IConfigurationBuilder builder) => new EtcdConfigurationProvider(_client, KeyPrefix);
	}
}