using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft.World.Chunk
{
	// Enums
	public enum HeightMapData : byte
	{
		None = 0,
		HasData = 1,
		TooHigh = 2,
		TooLow = 3,
		AllCopied = 4
	}

	public enum SubChunkRequestMode : uint
	{
		Limitless = uint.MaxValue,
		Limited = uint.MaxValue - 1
	}

	public enum SubChunkResult : byte
	{
		Success = 1,
		ChunkNotFound = 2,
		InvalidDimension = 3,
		PlayerNotFound = 4,
		IndexOutOfBounds = 5,
		SuccessAllAir = 6
	}

	// Structures
	public struct SubChunkOffset
	{
		public sbyte X { get; set; }
		public sbyte Y { get; set; }
		public sbyte Z { get; set; }

		public SubChunkOffset(sbyte x, sbyte y, sbyte z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}

	public struct SubChunkEntry
	{
		public SubChunkOffset Offset { get; set; }
		public byte Result { get; set; }
		public byte[] RawPayload { get; set; }
		public byte HeightMapType { get; set; }
		public sbyte[] HeightMapData { get; set; }
		public byte RenderHeightMapType { get; set; }
		public sbyte[] RenderHeightMapData { get; set; }
		public ulong BlobHash { get; set; }
	}
}
