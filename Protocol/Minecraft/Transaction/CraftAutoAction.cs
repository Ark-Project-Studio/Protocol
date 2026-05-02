namespace Protocol.Minecraft.Transaction;

public class CraftAutoAction : ItemStackAction
{
	public uint RecipeNetworkId { get; set; }
	public byte TimesCrafted { get; set; }
	public byte TimesCrafted2 { get; set; }
	public List<RecipeIngredient> Ingredients { get; set; } = new();
}