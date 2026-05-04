using Protocol.Minecraft.Inventory.Item;

namespace Protocol.Minecraft.Inventory.Transaction;

public abstract class TransactionRecord
{
	public int StackNetworkId { get; set; }

	public int Slot { get; set; }
	public NetworkItemStackDescriptor OldItem { get; set; }
	public NetworkItemStackDescriptor NewItem { get; set; }
}