using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft.Level.Chunk
{

	public enum SubChunkRequestMode : uint
	{
		Limitless = uint.MaxValue,
		Limited = uint.MaxValue - 1
	}

	public enum SubChunkResult : byte
	{
		Undefined = 0,
		Success = 1,
		LevelChunkDoesntExist = 2,
		WrongDimension = 3,
		PlayerDoesntExist = 4,
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
