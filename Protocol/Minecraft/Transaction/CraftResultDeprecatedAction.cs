namespace Protocol.Minecraft.Transaction;

public class CraftResultDeprecatedAction : ItemStackAction
{
	public NetworkItemInstanceDescriptor[] ResultItems { get; set; } = [];
	public byte TimesCrafted { get; set; }
}