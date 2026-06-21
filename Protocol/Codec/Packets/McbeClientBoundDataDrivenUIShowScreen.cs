using Protocol.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Codec.Packets
{
	public class McbeClientBoundDataDrivenUIShowScreen : Packet
	{
		public string ScreenId;
		public uint FormId;
		public Optional<uint> DataInstanceId;
		public McbeClientBoundDataDrivenUIShowScreen()
		{
			IsMcbe = true;
			Id = 333;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(ScreenId);
			Write(FormId);
			Write(DataInstanceId.HasValue);
			if (DataInstanceId.HasValue)
			{
				Write(DataInstanceId.Value);
			}
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			ScreenId = ReadString();
			FormId = ReadUint();
			if (ReadBool())
			{
				DataInstanceId = new Optional<uint>(ReadUint());
			}
		}
	}
}
