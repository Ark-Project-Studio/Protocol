using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets
{
	public class McbeCameraSpline : Packet
	{
		public CameraSplineDefinition[] Splines;
		public McbeCameraSpline()
		{
			IsMcbe = true;
			Id = 338;
		}
		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteSlice(Splines,Write);
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			Splines = ReadSlice(ReadCameraSplineDefinition);
		}
	}
}
