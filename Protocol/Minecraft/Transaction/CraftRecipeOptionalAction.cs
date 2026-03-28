namespace Protocol.Minecraft.Transaction;

public class CraftRecipeOptionalAction : ItemStackAction
{
	public uint RecipeNetworkId { get; set; }
	public int FilteredStringIndex { get; set; }
}