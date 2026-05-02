using System.Numerics;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Minecraft.Transaction;

public class ItemUseTransaction : Transaction
{
	public McbeInventoryTransaction.ItemUseActionType ActionType { get; set; }
	public McbeInventoryTransaction.TriggerType TriggerType { get; set; }
	public BlockCoordinates Position { get; set; }
	public int Face { get; set; }
	public int Slot { get; set; }
	public Item Item { get; set; }
	public Vector3 FromPosition { get; set; }
	public Vector3 ClickPosition { get; set; }
	public uint BlockRuntimeId { get; set; }
	public uint ClientPredictedResult { get; set; }
}