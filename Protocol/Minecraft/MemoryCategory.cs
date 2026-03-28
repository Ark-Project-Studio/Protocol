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
}
