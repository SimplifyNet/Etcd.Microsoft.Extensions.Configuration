using Etcd.Microsoft.Extensions.Configuration.Settings;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Etcd.Microsoft.Extensions.Configuration.Tests.Settings;

[TestFixture]
public class ConfigurationBasedEtcdSettingsTests
{
	[Test]
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
	}
}