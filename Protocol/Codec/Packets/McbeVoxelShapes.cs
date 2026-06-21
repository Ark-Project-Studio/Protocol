using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;
using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets
{
	public class McbeVoxelShapes : Packet
	{
	
		public ushort CustomShapeCount { get; set; }
		public System.Collections.Generic.List<VoxelShape> Shapes { get; set; }
		public System.Collections.Generic.List<VoxelShapeNameEntry> NameMap { get; set; }
		
		public McbeVoxelShapes()
		{
			IsMcbe = true;
			Id = 337;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteSlice(Shapes.ToArray(),Write);
			WriteSlice(NameMap.ToArray(),Write);
			Write(CustomShapeCount);
			
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			Shapes = ReadSlice(ReadVoxelShape).ToList();
			NameMap = ReadSlice(ReadVoxelShapeNameEntry).ToList();
			CustomShapeCount = ReadUshort();
		}
	}
}
