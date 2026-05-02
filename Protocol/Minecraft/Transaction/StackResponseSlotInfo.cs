namespace Protocol.Minecraft.Transaction;

public class StackResponseSlotInfo
{
	public byte RequestedSlot { get; set; }
	public byte Slot { get; set; }
	public byte Count { get; set; }
	public int StackNetworkId { get; set; }
	public string CustomName { get; set; }
	public string FilteredCustomName { get; set; }
	public int DurationCorrection { get; set; }

	public byte HotbarSlot
	{
		get => RequestedSlot;
		set => RequestedSlot = value;
	}

	public int DurabilityCorrection
	{
		get => DurationCorrection;
		set => DurationCorrection = value;
	}
}