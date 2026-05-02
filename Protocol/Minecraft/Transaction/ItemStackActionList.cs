namespace Protocol.Minecraft.Transaction;

public class ItemStackActionList : List<ItemStackAction>
{
	public int RequestId { get; set; }
	public List<string> StringsToFilter { get; set; } = new();
	public int StringsToFilterOrigin { get; set; }
}