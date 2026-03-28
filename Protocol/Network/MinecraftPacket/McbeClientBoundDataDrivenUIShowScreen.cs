using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network.MinecraftPacket
{
	public class McbeClientBoundDataDrivenUIShowScreen : Packet
	{
		public string ScreenId;
		public McbeClientBoundDataDrivenUIShowScreen()
		{
			IsMcbe = true;
			Id = 333;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(ScreenId);
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			ScreenId = ReadString();
		}
	}
}
