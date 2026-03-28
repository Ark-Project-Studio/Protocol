using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket
{
	public class McbeVoxelShapes : Packet
	{
	
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
			
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			Shapes = ReadSlice(ReadVoxelShape).ToList();
			NameMap = ReadSlice(ReadVoxelShapeNameEntry).ToList();
		}
	}
}
