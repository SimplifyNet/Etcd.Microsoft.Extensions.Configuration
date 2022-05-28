using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Settings;
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
				new EtcdSettings("https://serveraddress:2379"),
				"MyPrefix")
			.Build();

		// Act

		var testSection = config.GetSection("TestSection");
		var testSubSection = testSection.GetSection("SubSection");
		var list = testSection.GetSection("ArraySection").Get<List<string>>();
		var testAppSection = config.GetSection("TestAppSection");

		// Assert

		Assert.IsNotNull(config);
		Assert.IsTrue(config.GetChildren().Any());

		Assert.AreEqual("Item 1 value", testSection["Item1"]);
		Assert.AreEqual("Item 2 value", testSection["Item2"]);
		Assert.AreEqual("Sub section value 1", testSubSection["Item1"]);
		Assert.AreEqual("Sub section value 2", testSubSection["Item2"]);
		Assert.AreEqual(2, list.Count);
		Assert.AreEqual("Item 1", list[0]);
		Assert.AreEqual("Item 2", list[1]);
		Assert.AreEqual("1234321", testAppSection["Item1"]);
	}
}