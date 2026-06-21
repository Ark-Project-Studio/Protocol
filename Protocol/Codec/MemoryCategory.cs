using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft
{
	public struct MemoryCategoryCounter
	{
		public byte Category { get; set; }
		public ulong Bytes { get; set; }
	}

	public struct EntityDiagnosticTimingInfo
	{
		public string DisplayName { get; set; }
		public string Entity { get; set; }
		public ulong TimeInNS { get; set; }
		public byte PercentOfTotal { get; set; }
	}

	public struct SystemDiagnosticTimingInfo
	{
		public string DisplayName { get; set; }
		public ulong SystemIndex { get; set; }
		public ulong TimeInNS { get; set; }
		public byte PercentOfTotal { get; set; }
	}
}
