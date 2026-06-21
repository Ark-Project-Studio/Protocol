using Protocol.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Codec.Packets
{
	public class McbeTickSync : Packet
	{
		public long req_time;
		public long res_time;
		public McbeTickSync()
		{
			Id = 23;
			IsMcbe = true;
		}
		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(req_time);
			Write(res_time);
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			req_time = ReadLong();
			res_time = ReadLong();
		}
	}
}
