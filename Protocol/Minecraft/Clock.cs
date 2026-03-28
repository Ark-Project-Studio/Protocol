using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft
{
	public enum ClockPayloadType
	{
		SyncState = 0,
		InitializeRegistry = 1,
		AddTimeMarker = 2,
		RemoveTimeMarker = 3
	}

	public struct SyncWorldClockStateData
	{
		public ulong ClockID { get; set; }
		public int Time { get; set; }
		public bool Paused { get; set; }
	}

	public struct TimeMarkerData
	{
		public ulong ID { get; set; }
		public string Name { get; set; }
		public int Time { get; set; }
		public Optional<int> Period { get; set; }
	}

	public struct WorldClockData
	{
		public ulong ID { get; set; }
		public string Name { get; set; }
		public int Time { get; set; }
		public bool Paused { get; set; }
		public System.Collections.Generic.List<TimeMarkerData> TimeMarkers { get; set; }
	}
}
