namespace Protocol.Minecraft.Transaction;

public class StackResponseContainerInfo
{
	public FullContainerName ContainerName { get; set; } = new();
	public byte ContainerId
	{
		get => ContainerName.ContainerID;
		set => ContainerName.ContainerID = value;
	}
	public int DynamicId
	{
		get => ContainerName.DynamicContainerID.HasValue ? (int)ContainerName.DynamicContainerID.Value : 0;
		set => ContainerName.DynamicContainerID = value == 0 ? new Optional<uint>() : new Optional<uint>((uint)value);
	}
	public List<StackResponseSlotInfo> Slots { get; set; } = new();
}