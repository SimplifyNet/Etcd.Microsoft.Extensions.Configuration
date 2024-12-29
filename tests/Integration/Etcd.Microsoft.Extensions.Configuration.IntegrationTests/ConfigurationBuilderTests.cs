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

		var config = new ConfigurationBuilder()
			.AddEtcd(
				new Credentials("MyUserName", "passw"),
				new EtcdSettings("http://localhost:2380"),
				"MyPrefix")
			.Build();

		// Act

		var testSection = config.GetSection("TestSection");
		var testSubSection = testSection.GetSection("SubSection");
		var list = testSection.GetSection("ArraySection").Get<List<string>>();
		var testAppSection = config.GetSection("TestAppSection");

		// Assert

		Assert.That(config, Is.Not.Null);
		Assert.That(config.GetChildren().Any());
		Assert.That(testAppSection.GetChildren().Any());

		Assert.That(testSection["Item1"], Is.EqualTo("Item 1 value"));
		Assert.That(testSection["Item2"], Is.EqualTo("Item 2 value"));
		Assert.That(testSubSection["Item1"], Is.EqualTo("Sub section value 1"));
		Assert.That(testSubSection["Item2"], Is.EqualTo("Sub section value 2"));
		Assert.That(list.Count, Is.EqualTo(2));
		Assert.That(list[0], Is.EqualTo("Item 1"));
		Assert.That(list[1], Is.EqualTo("Item 2"));
		Assert.That(testAppSection["Item1"], Is.EqualTo("1234321"));
	}
}