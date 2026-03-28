using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network.MinecraftPacket
{
	public enum DataDrivenScreenCloseReason : byte
	{
		ProgrammaticClose = 0,
		ProgrammaticCloseAll = 1,
		ClientCanceled = 2,
		UserBusy = 3,
		InvalidForm = 4
	}
	public class McbeServerBoundDataDrivenScreenClosed : Packet
	{
		
		public Optional<uint> FormID { get; set; }
		public byte CloseReason { get; set; }
		public McbeServerBoundDataDrivenScreenClosed()
		{
			Id = 343;
			IsMcbe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(FormID.HasValue);
			if (FormID.HasValue)
			{
				Write(FormID.Value);
			}
			Write(CloseReason);
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			
			if (ReadBool())
			{
				FormID = new Optional<uint>(ReadUint());
			}
			CloseReason = ReadByte();
		}
	}
}
