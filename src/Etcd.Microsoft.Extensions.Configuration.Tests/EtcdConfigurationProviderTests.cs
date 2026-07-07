using System;
using System.Collections.Generic;
using Etcd.Microsoft.Extensions.Configuration.Client;
using Etcd.Microsoft.Extensions.Configuration.Settings;
using Etcd.Microsoft.Extensions.Configuration.Watch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;

#nullable enable

namespace Etcd.Microsoft.Extensions.Configuration.Tests;

[TestFixture]
public class EtcdConfigurationProviderTests
{
    [Test]
    public void WatchCallback_PutSameValue_DoesNotReload()
    {
        // Arrange
        var client = new TestEtcdKeyValueClient(new Dictionary<string, string?>
        {
            ["key"] = "value"
        });

        var provider = new EtcdConfigurationProvider(client);
        provider.Load();

        var reloadCount = SubscribeReloadCounter(provider);

        // Act
        client.RaiseWatchCallback(new WatchEvent(EventType.Put, "key", "value"));

        // Assert
        Assert.That(reloadCount(), Is.Zero);
        Assert.That(provider.TryGet("key", out var value), Is.True);
        Assert.That(value, Is.EqualTo("value"));
    }

    [Test]
    public void WatchCallback_DeleteMissingKey_DoesNotReload()
    {
        // Arrange
        var client = new TestEtcdKeyValueClient(new Dictionary<string, string?>
        {
            ["key"] = "value"
        });

        var provider = new EtcdConfigurationProvider(client);
        provider.Load();

        var reloadCount = SubscribeReloadCounter(provider);

        // Act
        client.RaiseWatchCallback(new WatchEvent(EventType.Delete, "missing", string.Empty));

        // Assert
        Assert.That(reloadCount(), Is.Zero);
        Assert.That(provider.TryGet("key", out var value), Is.True);
        Assert.That(value, Is.EqualTo("value"));
    }

    [Test]
    public void WatchCallback_PutChangedValue_ReloadsAndUpdatesValue()
    {
        // Arrange
        var client = new TestEtcdKeyValueClient(new Dictionary<string, string?>
        {
            ["key"] = "old-value"
        });

        var provider = new EtcdConfigurationProvider(client);
        provider.Load();

        var reloadCount = SubscribeReloadCounter(provider);

        // Act
        client.RaiseWatchCallback(new WatchEvent(EventType.Put, "key", "new-value"));

        // Assert
        Assert.That(reloadCount(), Is.EqualTo(1));
        Assert.That(provider.TryGet("key", out var value), Is.True);
        Assert.That(value, Is.EqualTo("new-value"));
    }

    [Test]
    public void WatchCallback_DeleteExistingKey_ReloadsAndRemovesValue()
    {
        // Arrange
        var client = new TestEtcdKeyValueClient(new Dictionary<string, string?>
        {
            ["key"] = "value"
        });

        var provider = new EtcdConfigurationProvider(client);
        provider.Load();

        var reloadCount = SubscribeReloadCounter(provider);

        // Act
        client.RaiseWatchCallback(new WatchEvent(EventType.Delete, "key", string.Empty));

        // Assert
        Assert.That(reloadCount(), Is.EqualTo(1));
        Assert.That(provider.TryGet("key", out _), Is.False);
    }

    private static Func<int> SubscribeReloadCounter(IConfigurationProvider provider)
    {
        var reloadCount = 0;
        ChangeToken.OnChange(provider.GetReloadToken, () => reloadCount++);

        return () => reloadCount;
    }

    private sealed class TestEtcdKeyValueClient(IDictionary<string, string?> data) : IEtcdKeyValueClient
    {
        public event WatchHandler? WatchCallback;

        public IEtcdSettings Settings { get; } = new EtcdSettings("http://localhost:2379");

        public IDictionary<string, string?> GetAllKeys() => new Dictionary<string, string?>(data);

        public string? GetValue(string key) => data.TryGetValue(key, out var value) ? value : null;

        public void RaiseWatchCallback(params WatchEvent[] events) => WatchCallback?.Invoke(events);

        public void Dispose()
        {
        }
    }
}