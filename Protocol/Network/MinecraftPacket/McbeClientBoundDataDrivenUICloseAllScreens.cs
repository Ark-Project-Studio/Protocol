using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network.MinecraftPacket
{
	public class McbeClientBoundDataDrivenUICloseAllScreens : Packet
	{
		public McbeClientBoundDataDrivenUICloseAllScreens()
		{
			IsMcbe = true;
			Id = 334;
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
