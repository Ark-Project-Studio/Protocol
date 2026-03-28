using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network.MinecraftPacket
{
	public class McbePartyChanged : Packet
	{
		public string PartyID;
		public McbePartyChanged()
		{
			Id = 342;
			IsMcbe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(PartyID);
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			PartyID = ReadString();
		}
	}
}
