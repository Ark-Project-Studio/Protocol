namespace Protocol.Network.MinecraftPacket;

public class McbeCompletedUsingItem : Packet
{
	public enum UseMethodType
	{
		UseItemEquipArmour = 0,
		UseItemEat = 1,
		UseItemAttack = 2,
		UseItemConsume = 3,
		UseItemThrow = 4,
		UseItemShoot = 5,
		UseItemPlace = 6,
		UseItemFillBottle = 7,
		UseItemFillBucket = 8,
		UseItemPourBucket = 9,
		UseItemUseTool = 10,
		UseItemInteract = 11,
		UseItemRetrieved = 12,
		UseItemDyed = 13,
		UseItemTraded = 14,
		UseItemBrushingCompleted = 15,
		UseItemOpenedVault = 16
	}

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
