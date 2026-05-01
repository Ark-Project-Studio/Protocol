namespace Protocol.Minecraft
{
	public class HeightMapData
	{
		public short[] Heights { get; }

		public HeightMapData(short[] heights)
		{
			if (heights.Length != 256)
				throw new ArgumentException("Expected 256 data entries");

			Heights = heights;
		}

		public int GetHeight(int x, int z)
		{
			return Heights[((z & 0xf) << 4) | (x & 0xf)];
		}

		public bool IsAllTooLow => Heights.Any(x => x > 0);
		public bool IsAllTooHigh => Heights.Any(x => x <= 15);
	}

	public enum SubChunkPacketHeightMapType : byte
	{
		NoData = 0,
		HasData = 1,
		AllTooHigh = 2,
		AllTooLow = 3,
		AllCopied = 4
	}

	public enum SubChunkRequestResult : byte
	{
		Undefined = 0,
		Success = 1,
		LevelChunkDoesntExist = 2,
		WrongDimension = 3,
		PlayerDoesntExist = 4,
		IndexOutOfBounds = 5,
		SuccessAllAir = 6
	}
}