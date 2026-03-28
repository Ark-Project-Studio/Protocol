namespace Protocol.Minecraft.Transaction;

public class SwapAction : ItemStackAction
{
	public StackRequestSlotInfo Source { get; set; }
	public StackRequestSlotInfo Destination { get; set; }
}