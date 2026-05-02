namespace Protocol.Network.MinecraftPacket;

public class McbeCompletedUsingItem : Packet
{

	public McbeCompletedUsingItem()
	{
		Id = 142;
		IsMcbe = true;
	}


	public short UsedItemID { get; set; }


	public int UseMethod { get; set; }

	protected override void EncodePacket()
	{
		base.EncodePacket();
		Write(UsedItemID);
		Write(UseMethod);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		UsedItemID = ReadShort();
		UseMethod = ReadInt();
	}
}
