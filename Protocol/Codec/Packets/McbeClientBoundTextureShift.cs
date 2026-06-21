using Protocol.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Codec.Packets
{
	public class McbeClientBoundTextureShift : Packet
	{
		public byte ActionID { get; set; }
		public string CollectionName { get; set; }
		public string FromStep { get; set; }
		public string ToStep { get; set; }
		public System.Collections.Generic.List<string> AllSteps { get; set; }
		public ulong CurrentLengthTicks { get; set; }
		public ulong TotalLengthTicks { get; set; }
		public bool Enabled { get; set; }
		public McbeClientBoundTextureShift()
		{
			IsMcbe = true;
			Id = 336;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(ActionID);
			Write(CollectionName);
			Write(FromStep);
			Write(ToStep);
			WriteSlice(AllSteps.ToArray(), Write);
			WriteUnsignedVarLong(CurrentLengthTicks);
			WriteUnsignedVarLong(TotalLengthTicks);
			Write(Enabled);
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			ActionID = ReadByte();
			CollectionName = ReadString();
			FromStep = ReadString();
			ToStep = ReadString();
			AllSteps = ReadSlice(ReadString).ToList();
			CurrentLengthTicks = ReadUnsignedVarLong();
			TotalLengthTicks = ReadUnsignedVarLong();
			Enabled = ReadBool();
		}
	}
}
