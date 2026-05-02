namespace Protocol.Network.MinecraftPacket;

public class McbeMultiPlayerSettings : Packet
{
	public enum Type : int
	{
		EnableMultiplayer = 0,
		DisableMultiplayer = 1,
		RefreshJoincode = 2
	}

	public McbeMultiPlayerSettings()
	{
		Id = 139;
		IsMcbe = true;
	}


	public Type ActionType { get; set; }

	protected override void EncodePacket()
	{
		base.EncodePacket();
		WriteSignedVarInt((int)ActionType);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		ActionType = (Type)ReadSignedVarInt();
	}
}
