namespace Protocol.Minecraft.Transaction;

public class CraftResultDeprecatedAction : ItemStackAction
{
	public NetworkItemInstanceDescriptors ResultItems { get; set; } = new();
	public byte TimesCrafted { get; set; }
}