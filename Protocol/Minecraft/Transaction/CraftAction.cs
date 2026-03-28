namespace Protocol.Minecraft.Transaction;

public class CraftAction : ItemStackAction
{
	public uint RecipeNetworkId { get; set; }
	public byte TimesCrafted { get; set; }
}