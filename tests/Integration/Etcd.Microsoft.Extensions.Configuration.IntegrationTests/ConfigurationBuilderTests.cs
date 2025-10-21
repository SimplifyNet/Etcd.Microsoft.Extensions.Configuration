using System;
using System.Collections.Generic;
using System.Linq;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Etcd.Microsoft.Extensions.Configuration.IntegrationTests;

[TestFixture]
[Category("Integration")]
public class ConfigurationBuilderTests
{
	[Test]
	public void Build_WithSettingsFromEtcd_ValuesLoaded()
	{
		// Arrange

		var credentials = new Credentials("MyUserName", "passw");
		var etcdSettings = new EtcdSettings("http://localhost:2379");

		var config = new ConfigurationBuilder()
			.AddEtcd(credentials, etcdSettings)
			.AddEtcd(credentials, etcdSettings, "MyPrefix")
			.AddEtcd(credentials, etcdSettings, "MYCOMPLEX/prefix", "/")
			.Build();

		// Act
		PerformTest(config);
	}

	[Test]
	public void Build_WithSettingsFromEtcdAndCredentialsFromEnvironment_ValuesLoaded()
	{
		// Arrange

		Environment.SetEnvironmentVariable("ETCD_TEST_USERNAME", "MyUserName");
		Environment.SetEnvironmentVariable("ETCD_TEST_PASSWORD", "passw");

		var credentials = new Credentials("MyUserName", "passw");
		var envCredentials = Credentials.WithOverrideFromEnvironmentVariables("foo", "bar", "ETCD_TEST_USERNAME", "ETCD_TEST_PASSWORD");
		var envCredentials2 = Credentials.WithOverrideFromEnvironmentVariables("MyUserName", "bar", "ETCD_TEST_PASSWORD");

		var etcdSettings = new EtcdSettings("http://localhost:2379");

		var config = new ConfigurationBuilder()
			.AddEtcd(credentials, etcdSettings)
			.AddEtcd(envCredentials, etcdSettings, "MyPrefix")
			.AddEtcd(envCredentials2, etcdSettings, "MYCOMPLEX/prefix", "/")
			.Build();

		// Act
		PerformTest(config);

		// Assert
		Assert.Pass("Credentials info: " + envCredentials.ToString());
	}

	private static void PerformTest(IConfigurationRoot config)
	{
		var testSection = config.GetSection("TestSection");
		var testSubSection = testSection.GetSection("SubSection");
		var list = testSection.GetSection("ArraySection").Get<List<string>>();
		var testAppSection = config.GetSection("TestAppSection");
		var complexPrefixSection = config.GetSection("Settings");

		// Assert

		Assert.That(config, Is.Not.Null);
		Assert.That(config.GetChildren().Any());
		Assert.That(testAppSection.GetChildren().Any());
		Assert.That(complexPrefixSection.GetChildren().Any());

		Assert.That(testSection["Item1"], Is.EqualTo("Item 1 value"));
		Assert.That(testSection["iTem2"], Is.EqualTo("Item 2 value")); // Case insensitive key access
		Assert.That(testSubSection["Item1"], Is.EqualTo("Sub section value 1"));
		Assert.That(testSubSection["Item2"], Is.EqualTo("Sub section value 2"));
		Assert.That(list.Count, Is.EqualTo(2));
		Assert.That(list[0], Is.EqualTo("Item 1"));
		Assert.That(list[1], Is.EqualTo("Item 2"));
		Assert.That(testAppSection["Item1"], Is.EqualTo("1234321"));

		Assert.That(complexPrefixSection["TestKey"], Is.EqualTo("Test value")); // This method is just to have a breakpoint target for debugging purposes.
	}
}