namespace Protocol.Minecraft.Transaction;

public class CraftCreativeAction : ItemStackAction
{
	public uint CreativeItemNetworkId { get; set; }
	public byte ClientPredictedResult { get; set; }
}