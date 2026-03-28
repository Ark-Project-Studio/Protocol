namespace Protocol.Minecraft.Transaction;

public class ItemStackActionList : List<ItemStackAction>
{
	public int RequestId { get; set; }
	public List<string> filteredString { get; set; } = new();
	public int FilterCause { get; set; }
}