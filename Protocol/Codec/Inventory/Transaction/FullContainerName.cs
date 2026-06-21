namespace Protocol.Minecraft.Inventory.Transaction;

public class FullContainerName
{
	public FullContainerName()
	{
	}


	public FullContainerName(byte containerId)
	{
		ContainerID = containerId;
	}


	public FullContainerName(byte containerId, uint dynamicContainerId)
	{
		ContainerID = containerId;
		DynamicContainerID = new Optional<uint>(dynamicContainerId);
	}


	public byte ContainerID { get; set; }


	public Optional<uint>
		DynamicContainerID { get; set; }
}