using System.Numerics;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Minecraft.Transaction;

public class ItemUseOnEntityTransaction : Transaction
{
	public long EntityId { get; set; }
	public McbeInventoryTransaction.ItemUseOnActorActionType ActionType { get; set; }
	public int Slot { get; set; }
	public Item Item { get; set; }
	public Vector3 FromPosition { get; set; }
	public Vector3 ClickPosition { get; set; }
}