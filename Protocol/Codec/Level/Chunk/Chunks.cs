using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft.Level.Chunk
{
	public struct ChunkPos
	{
		private int _x;
		private int _z;

		public ChunkPos(int x, int z)
		{
			_x = x;
			_z = z;
		}

		public int X => _x;
		public int Z => _z;
	}

	public struct SubChunkPos
	{
		private int _x;
		private int _y;
		private int _z;

		public SubChunkPos(int x, int y, int z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public int X => _x;
		public int Y => _y;
		public int Z => _z;
	}
}
