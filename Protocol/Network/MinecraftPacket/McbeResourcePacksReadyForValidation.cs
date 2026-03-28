using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network.MinecraftPacket
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
