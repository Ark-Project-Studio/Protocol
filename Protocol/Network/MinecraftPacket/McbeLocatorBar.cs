using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket
{
	public class McbeLocatorBar : Packet
	{
		public List<LocatorBarWaypoint> Waypoints;
		public McbeLocatorBar()
		{
			Id = 341;
			IsMcbe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteSlice(Waypoints.ToArray(),Write);
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			Waypoints = ReadSlice(ReadLocatorBarWaypoint).ToList();
		}
	}
}
