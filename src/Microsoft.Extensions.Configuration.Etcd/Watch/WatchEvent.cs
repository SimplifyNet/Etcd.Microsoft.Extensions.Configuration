using System;

namespace Microsoft.Extensions.Configuration.Etcd.Watch
{
	/// <summary>
	/// Provides watch event
	/// </summary>
	public class WatchEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WatchEvent" /> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public WatchEvent(EventType type, string key, string value)
		{
			if (string.IsNullOrEmpty(key)) throw new ArgumentException("Value cannot be null or empty.", nameof(key));

			Type = type;
			Key = key;
			Value = value;
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>
		/// The type.
		/// </value>
		public EventType Type { get; }

		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>
		/// The key.
		/// </value>
		public string Key { get; }

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>
		/// The value.
		/// </value>
		public string Value { get; }
	}
}