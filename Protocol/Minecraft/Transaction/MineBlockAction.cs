namespace Protocol.Minecraft.Transaction;

public class MineBlockAction : ItemStackAction
{
	public int Slot { get; set; }
	public int Durability { get; set; }
	public int stackNetworkId { get; set; }
}