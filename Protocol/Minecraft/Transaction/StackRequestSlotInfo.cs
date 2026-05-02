namespace Protocol.Minecraft.Transaction;

public class StackRequestSlotInfo
{
	public FullContainerName FullContainerName { get; set; } = new();
	public byte ContainerId
	{
		get => FullContainerName.ContainerID;
		set => FullContainerName.ContainerID = value;
	}
	public byte Slot { get; set; }
	public int StackNetworkId { get; set; }
	public int DynamicId
	{
		get => FullContainerName.DynamicContainerID.HasValue ? (int)FullContainerName.DynamicContainerID.Value : 0;
		set => FullContainerName.DynamicContainerID = value == 0 ? new Optional<uint>() : new Optional<uint>((uint)value);
	}
}