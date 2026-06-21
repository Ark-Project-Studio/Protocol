using Protocol.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Codec.Packets
{
	public class McbeResourcePacksReadyForValidation : Packet
	{
		public McbeResourcePacksReadyForValidation()
		{
			Id = 340;
			IsMcbe = true;
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
		}
	}
}
