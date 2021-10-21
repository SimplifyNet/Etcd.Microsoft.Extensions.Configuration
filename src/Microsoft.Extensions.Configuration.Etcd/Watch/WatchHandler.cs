using System.Collections.Generic;

namespace Microsoft.Extensions.Configuration.Etcd.Watch
{
	/// <summary>
	/// Describes watch events delegate
	/// </summary>
	/// <param name="events">The events.</param>
	public delegate void WatchHandler(IEnumerable<WatchEvent> events);
}