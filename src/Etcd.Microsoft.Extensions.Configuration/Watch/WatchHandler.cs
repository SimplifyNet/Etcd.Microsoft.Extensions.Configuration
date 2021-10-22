using System.Collections.Generic;

namespace Etcd.Microsoft.Extensions.Configuration.Watch
{
	/// <summary>
	/// Describes watch events delegate
	/// </summary>
	/// <param name="events">The events.</param>
	public delegate void WatchHandler(IEnumerable<WatchEvent> events);
}