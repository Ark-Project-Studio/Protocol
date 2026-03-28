namespace Protocol.Minecraft.Transaction;

public class StackRequestSlotInfo
{
	public byte ContainerId { get; set; }
	public byte Slot { get; set; }
	public int StackNetworkId { get; set; }
	public int DynamicId { get; set; }
}