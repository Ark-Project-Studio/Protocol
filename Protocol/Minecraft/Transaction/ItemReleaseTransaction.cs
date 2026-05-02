using System.Numerics;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Minecraft.Transaction;

public class ItemReleaseTransaction : Transaction
{
	public McbeInventoryTransaction.ItemReleaseActionType ActionType { get; set; }
	public int Slot { get; set; }
	public NetworkItemStackDescriptor Item { get; set; }
	public Vector3 FromPosition { get; set; }
}