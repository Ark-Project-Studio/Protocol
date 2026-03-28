namespace Protocol.Minecraft.Transaction;

public class LoomStackRequestAction : ItemStackAction
{
	public string PatternId { get; set; }
	public byte TimesCrafted { get; set; }
}