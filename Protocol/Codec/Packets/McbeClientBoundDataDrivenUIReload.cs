using Protocol.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Codec.Packets
{
	public class McbeClientBoundDataDrivenUIReload : Packet
	{
		public McbeClientBoundDataDrivenUIReload()
		{
			IsMcbe = true;
			Id = 335;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
		}
	}
}
