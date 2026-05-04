using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;
using Protocol.Minecraft.Level.Block;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(VoxelCells value)
		{
			Write(value.XSize);
			Write(value.YSize);
			Write(value.ZSize);
			WriteSlice(value.Storage.ToArray(), Write);
		}

		public VoxelCells ReadVoxelCells()
		{
			var cells = new VoxelCells()
			{
				XSize = ReadByte(),
				YSize = ReadByte(),
				ZSize = ReadByte()
			};

			var storageArray = ReadSlice(ReadByte);
			cells.Storage = new System.Collections.Generic.List<byte>(storageArray);

			return cells;
		}

		public void Write(VoxelShapeNameEntry value)
		{
			Write(value.Name);
			Write(value.ID);
		}

		public VoxelShapeNameEntry ReadVoxelShapeNameEntry()
		{
			return new VoxelShapeNameEntry
			{
				Name = ReadString(),
				ID = ReadUshort()
			};
		}

		public void Write(VoxelShape value)
		{
			Write(value.Cells);
			WriteSlice(value.XCoordinates.ToArray(), Write);
			WriteSlice(value.YCoordinates.ToArray(), Write);
			WriteSlice(value.ZCoordinates.ToArray(), Write);
		}

		public VoxelShape ReadVoxelShape()
		{
			var shape = new VoxelShape
			{
				Cells = ReadVoxelCells()
			};

			var xArray = ReadSlice(ReadFloat);
			shape.XCoordinates = new System.Collections.Generic.List<float>(xArray);

			var yArray = ReadSlice(ReadFloat);
			shape.YCoordinates = new System.Collections.Generic.List<float>(yArray);

			var zArray = ReadSlice(ReadFloat);
			shape.ZCoordinates = new System.Collections.Generic.List<float>(zArray);

			return shape;
		}
	}
}
