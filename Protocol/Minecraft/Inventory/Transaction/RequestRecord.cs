namespace Protocol.Minecraft.Inventory.Transaction;

public class RequestRecord
{
	public byte ContainerId { get; set; }
	public List<byte> Slots { get; set; } = new();
}