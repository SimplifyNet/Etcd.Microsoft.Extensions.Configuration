using Etcd.Microsoft.Extensions.Configuration.Settings;
using NUnit.Framework;

namespace Etcd.Microsoft.Extensions.Configuration.Tests.Settings
{
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

			Assert.AreEqual("http://localhost:2379", settings.ConnectionString);
			Assert.AreEqual("1234321", settings.CertificateData);
		}
	}
}