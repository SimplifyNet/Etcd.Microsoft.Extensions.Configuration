using Etcd.Microsoft.Extensions.Configuration.Settings;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Etcd.Microsoft.Extensions.Configuration.IntegrationTests.Core.Settings;

[TestFixture]
public class ConfigurationBasedEtcdSettingsTests
{
	[Test]
	[Category("Integration")]
	public void Ctor_ExistingSettings_Loaded()
	{
		// Arrange

		var config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();

		// Act

		var settings = new ConfigurationBasedEtcdSettings(config);

		// Assert

		Assert.That(settings.ConnectionString, Is.EqualTo("http://localhost:2379"));
		Assert.That(settings.CertificateData, Is.EqualTo("1234321"));
	}
}