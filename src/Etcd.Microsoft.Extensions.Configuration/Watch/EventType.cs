namespace Etcd.Microsoft.Extensions.Configuration.Watch;

/// <summary>
/// Provides key-value event type
/// </summary>
public enum EventType
{
	/// <summary>
	/// The put key event
	/// </summary>
	Put,

	/// <summary>
	/// The delete key event
	/// </summary>
	Delete
}