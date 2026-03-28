namespace Protocol.Minecraft.Transaction;

public class GrindstoneStackRequestAction : ItemStackAction
{
	public uint RecipeNetworkId { get; set; }
	public int RepairCost { get; set; }
	public byte TimesCrafted { get; set; }
}