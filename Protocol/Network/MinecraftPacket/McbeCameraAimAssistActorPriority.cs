using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;
using Protocol.Minecraft.Camera;

namespace Protocol.Network.MinecraftPacket
{
	public partial class McbeCameraAimAssistActorPriority : Packet
	{
		public CameraAimAssistActorPriorityData[] CameraAimAssistActorPriority { get; set; }
		public McbeCameraAimAssistActorPriority()
		{
			IsMcbe = true;
			Id = 339;
		}
		protected override void DecodePacket()
		{
			base.DecodePacket();
			CameraAimAssistActorPriority = ReadSlice(ReadCameraAimAssistActorPriorityData);
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteSlice(CameraAimAssistActorPriority,Write);
		}
	}

	
}
