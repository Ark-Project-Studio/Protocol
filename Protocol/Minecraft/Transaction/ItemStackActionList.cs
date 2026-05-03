namespace Protocol.Minecraft.Transaction;

public class ItemStackActionList
{
	public int RequestId { get; set; }
	public ItemStackAction[] Actions { get; set; } = [];
	public List<string> StringsToFilter { get; set; } = new();
	public int StringsToFilterOrigin { get; set; }
}