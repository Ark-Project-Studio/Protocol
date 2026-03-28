namespace Protocol.Network.MinecraftPacket;

public class McbeMultiPlayerSettings : Packet
{
	public enum Action
	{
		EnableMultiPlayer = 0,
		DisableMultiPlayer = 1,
		RefreshJoinCode = 2
	}

	public McbeMultiPlayerSettings()
	{
		Id = 139;
		IsMcbe = true;
	}


	public int ActionType { get; set; }

	protected override void EncodePacket()
	{
		base.EncodePacket();
		WriteSignedVarInt(ActionType);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		ActionType = ReadSignedVarInt();
	}
}
