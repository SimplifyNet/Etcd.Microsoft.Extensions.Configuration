using System;
using System.Collections.Generic;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Client;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Microsoft.Extensions.Configuration;

namespace Etcd.Microsoft.Extensions.Configuration;

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
	/// <param name="keyPrefix">Key prefix for additional configuration layer with prefix.</param>
	/// <param name="enableWatch">if set to <c>true</c> the keys watch will be enabled.</param>
	/// <param name="unwatchOnDispose">if set to <c>true</c> the watching keys will be unwatched on dispose .</param>
	/// <returns></returns>
	public static IConfigurationBuilder AddEtcd(this IConfigurationBuilder configurationBuilder,
		ICredentials credentials,
		string? keyPrefix,
		bool enableWatch = true,
		bool unwatchOnDispose = true) =>
		configurationBuilder.AddEtcd(credentials, null, keyPrefix, enableWatch, unwatchOnDispose);

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
		IEtcdSettings? settings,
		string? keyPrefix,
		bool enableWatch = true,
		bool unwatchOnDispose = true) =>
		configurationBuilder.AddEtcd(credentials, settings, !string.IsNullOrEmpty(keyPrefix)
				? new List<string>
				{
					keyPrefix!
				}
				: null,
			enableWatch, unwatchOnDispose);

	/// <summary>
	/// Adds the etcd provider to the configuration builder.
	/// </summary>
	/// <param name="configurationBuilder">The configuration builder.</param>
	/// <param name="credentials">The credentials.</param>
	/// <param name="keyPrefixes">The key prefixes for additional configuration layers with prefixes.</param>
	/// <param name="enableWatch">if set to <c>true</c> the keys watch will be enabled.</param>
	/// <param name="unwatchOnDispose">if set to <c>true</c> the watching keys will be unwatched on dispose .</param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException">configurationBuilder</exception>
	public static IConfigurationBuilder AddEtcd(this IConfigurationBuilder configurationBuilder,
		ICredentials credentials,
		IList<string> keyPrefixes,
		bool enableWatch = true,
		bool unwatchOnDispose = true) =>
		configurationBuilder.AddEtcd(credentials, null, keyPrefixes, enableWatch, unwatchOnDispose);

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
	/// <exception cref="ArgumentNullException">configurationBuilder</exception>
	public static IConfigurationBuilder AddEtcd(this IConfigurationBuilder configurationBuilder,
		ICredentials credentials,
		IEtcdSettings? settings = null,
		IList<string>? keyPrefixes = null,
		bool enableWatch = true,
		bool unwatchOnDispose = true)
	{
		ArgumentNullException.ThrowIfNull(configurationBuilder);

		var clientFactory = new EtcdClientFactory(settings);
		var client = new EtcdKeyValueClient(clientFactory, credentials, enableWatch, unwatchOnDispose);

		if (keyPrefixes != null)
			foreach (var keyPrefix in keyPrefixes)
				configurationBuilder.Add(new EtcdConfigurationSource(client, keyPrefix));
		else
			configurationBuilder.Add(new EtcdConfigurationSource(client));

		return configurationBuilder;
	}
}