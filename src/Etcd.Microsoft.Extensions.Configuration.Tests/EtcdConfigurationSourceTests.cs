using System;
using System.Collections.Generic;
using System.Threading;
using dotnet_etcd.interfaces;
using Etcd.Microsoft.Extensions.Configuration.Auth;
using Etcd.Microsoft.Extensions.Configuration.Client;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Etcdserverpb;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Etcd.Microsoft.Extensions.Configuration.Tests;

[TestFixture]
public class EtcdConfigurationSourceTests
{
	[Test]
	public void Build_MultipleProviders_CreatesIndependentClients()
	{
		// Arrange

		var clients = new List<Mock<IEtcdClient>>();
		var clientFactory = CreateClientFactory(clients);

		var source = new EtcdConfigurationSource(
			new EtcdConfigurationSourceOptions(clientFactory.Object, new Credentials("user", "password")));

		// Act
		var first = source.Build(new ConfigurationBuilder());
		var second = source.Build(new ConfigurationBuilder());

		// Assert

		Assert.That(clients, Has.Count.EqualTo(2));
		Assert.That(first, Is.Not.SameAs(second));

		((IDisposable)first).Dispose();

		clients[0].Verify(x => x.Dispose(), Times.Once);
		clients[1].Verify(x => x.Dispose(), Times.Never);
	}

	[Test]
	public void Build_AfterPreviousProviderDisposed_CreatesNewClient()
	{
		// Arrange

		var clients = new List<Mock<IEtcdClient>>();
		var clientFactory = CreateClientFactory(clients);

		var source = new EtcdConfigurationSource(
			new EtcdConfigurationSourceOptions(clientFactory.Object, new Credentials("user", "password")));

		var first = source.Build(new ConfigurationBuilder());
		((IDisposable)first).Dispose();

		// Act
		var second = source.Build(new ConfigurationBuilder());

		// Assert

		Assert.That(clients, Has.Count.EqualTo(2));
		Assert.That(second, Is.Not.SameAs(first));

		clients[0].Verify(x => x.Dispose(), Times.Once);
		clients[1].Verify(x => x.Dispose(), Times.Never);
	}

	private static Mock<IEtcdClientFactory> CreateClientFactory(ICollection<Mock<IEtcdClient>> clients)
	{
		var clientFactory = new Mock<IEtcdClientFactory>();

		clientFactory.SetupGet(x => x.Settings).Returns(new EtcdSettings("http://localhost:2379"));

		clientFactory.Setup(x => x.Create()).Returns(() =>
		{
			var client = new Mock<IEtcdClient>();
			client.Setup(x => x.Authenticate(
					It.IsAny<AuthenticateRequest>(),
					It.IsAny<Metadata>(),
					It.IsAny<DateTime?>(),
					It.IsAny<CancellationToken>()))
				.Returns(new AuthenticateResponse
				{
					Token = "token"
				});

			clients.Add(client);

			return client.Object;
		});

		return clientFactory;
	}
}
