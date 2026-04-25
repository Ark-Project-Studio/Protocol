using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network.MinecraftPacket
{
	public class McbeClientBoundDataDrivenUICloseAllScreens : Packet
	{
		public Optional<uint> formId;

		public McbeClientBoundDataDrivenUICloseAllScreens()
		{
			IsMcbe = true;
			Id = 334;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(formId.HasValue);
			if (formId.HasValue)
			{
				Write(formId.Value);
			}
			
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			bool hasFormId = ReadBool();
			if (hasFormId)
			{
				formId = new Optional<uint>(ReadUint());
			}
		}
	}
}
