using Protocol.Network.MinecraftPacket;

namespace Protocol.Minecraft.Transaction;

public class CraftTransactionRecord : TransactionRecord
{
	public McbeInventoryTransaction.CraftingAction Action { get; set; }
}