using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft
{
	public struct VoxelCells
	{
		public byte XSize { get; set; }
		public byte YSize { get; set; }
		public byte ZSize { get; set; }
		public System.Collections.Generic.List<byte> Storage { get; set; }
	}

	public struct VoxelShape
	{
		public VoxelCells Cells { get; set; }
		public System.Collections.Generic.List<float> XCoordinates { get; set; }
		public System.Collections.Generic.List<float> YCoordinates { get; set; }
		public System.Collections.Generic.List<float> ZCoordinates { get; set; }
	}
	public struct VoxelShapeNameEntry
	{
		public string Name { get; set; }
		public ushort ID { get; set; }
	}
}
