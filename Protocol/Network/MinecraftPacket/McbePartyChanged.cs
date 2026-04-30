namespace Protocol.Network.MinecraftPacket
{
	public class PartyPlayerInfo
	{
		public string PartyID { get; set; } = string.Empty;
		public bool IsPartyLeader { get; set; }
	}

	public class McbePartyChanged : Packet
	{
		public Optional<PartyPlayerInfo> PlayerPartyInfo { get; set; } = new();

		public McbePartyChanged()
		{
			Id = 342;
			IsMcbe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			Write(PlayerPartyInfo.HasValue);
			if (PlayerPartyInfo.HasValue)
			{
				Write(PlayerPartyInfo.Value.PartyID);
				Write(PlayerPartyInfo.Value.IsPartyLeader);
			}
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			if (ReadBool())
			{
				PlayerPartyInfo = new Optional<PartyPlayerInfo>(new PartyPlayerInfo
				{
					PartyID = ReadString(),
					IsPartyLeader = ReadBool()
				});
			}
		}
	}
}
