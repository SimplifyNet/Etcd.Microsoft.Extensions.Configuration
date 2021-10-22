using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration.Etcd.Auth;
using Microsoft.Extensions.Configuration.Etcd.Client;
using Microsoft.Extensions.Configuration.Etcd.Settings;

namespace Microsoft.Extensions.Configuration.Etcd
{
	/// <summary>
	/// Provides etcd-based configuration extensions
	/// </summary>
	public static class ConfigurationBuilderExtensions
	{
		/// <summary>
		/// Adds the etcd provider to the configuration builder.
		/// </summary>
		/// <param name="configurationBuilder">The configuration builder.</param>
		/// <param name="credentials">The credentials.</param>
		/// <param name="settings">The settings.</param>
		/// <param name="keyPrefix">Key prefix for additional configuration layer with prefix.</param>
		/// <param name="enableWatch">if set to <c>true</c> the keys watch will be enabled.</param>
		/// <param name="unwatchOnDispose">if set to <c>true</c> the watching keys will be unwatched on dispose .</param>
		/// <returns></returns>
		public static IConfigurationBuilder AddEtcd(this IConfigurationBuilder configurationBuilder,
			ICredentials credentials,
			IEtcdSettings? settings = null,
			string? keyPrefix = null,
			bool enableWatch = true,
			bool unwatchOnDispose = true)
		{
			return configurationBuilder.AddEtcd(credentials, settings, !string.IsNullOrEmpty(keyPrefix)
					? new List<string>
					{
						keyPrefix!
					}
					: null,
				enableWatch, unwatchOnDispose);
		}

		/// <summary>
		/// Adds the etcd provider to the configuration builder.
		/// </summary>
		/// <param name="configurationBuilder">The configuration builder.</param>
		/// <param name="credentials">The credentials.</param>
		/// <param name="settings">The settings.</param>
		/// <param name="keyPrefixes">The key prefixes for additional configuration layers with prefixes.</param>
		/// <param name="enableWatch">if set to <c>true</c> the keys watch will be enabled.</param>
		/// <param name="unwatchOnDispose">if set to <c>true</c> the watching keys will be unwatched on dispose .</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentNullException">configurationBuilder</exception>
		public static IConfigurationBuilder AddEtcd(this IConfigurationBuilder configurationBuilder,
			ICredentials credentials,
			IEtcdSettings? settings = null,
			IList<string>? keyPrefixes = null,
			bool enableWatch = true,
			bool unwatchOnDispose = true)
		{
			if (configurationBuilder == null)
				throw new ArgumentNullException(nameof(configurationBuilder));

			var clientFactory = new EtcdClientFactory(settings);
			var client = new EtcdKeyValueClient(clientFactory, credentials, enableWatch, unwatchOnDispose);

			configurationBuilder.Add(new EtcdConfigurationSource(client));

			if (keyPrefixes != null)
				foreach (var keyPrefix in keyPrefixes)
					configurationBuilder.Add(new EtcdConfigurationSource(client, keyPrefix));

			return configurationBuilder;
		}
	}
}