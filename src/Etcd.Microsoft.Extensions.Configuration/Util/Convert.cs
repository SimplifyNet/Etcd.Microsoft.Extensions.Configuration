using System;
using System.Collections.Generic;
using System.Linq;
using Etcd.Microsoft.Extensions.Configuration.Watch;
using Mvccpb;

namespace Etcd.Microsoft.Extensions.Configuration.Util;

/// <summary>
/// Represents etcd convert utilities
/// </summary>
public static class Convert
{
	/// <summary>
	/// Converts the KeyValue list  to dictionary.
	/// </summary>
	/// <param name="list">The list.</param>
	public static IDictionary<string, string?> ToDictionary(IList<KeyValue> list) => list.ToDictionary(x => x.Key.ToStringUtf8(), x => x.Value?.ToStringUtf8());

	/// <summary>
	/// Converts dotnet-etcd EventType to to local EventType.
	/// </summary>
	/// <param name="sourceType">Type of the source.</param>
	public static EventType ToEventType(Event.Types.EventType sourceType) =>
		sourceType switch
		{
			Event.Types.EventType.Put => EventType.Put,
			Event.Types.EventType.Delete => EventType.Delete,
			_ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, null)
		};
}