namespace Protocol.Minecraft.Inventory.Item
{
	public class CreativeContentWriteEntry
	{
		public uint NetId { get; set; }
		public NetworkItemInstanceDescriptor Item { get; set; } = NetworkItemInstanceDescriptor.Empty;
		public uint GroupIndex { get; set; }

		public CreativeContentWriteEntry()
		{
		}

		public CreativeContentWriteEntry(uint netId, NetworkItemInstanceDescriptor item, uint groupIndex)
		{
			NetId = netId;
			Item = item;
			GroupIndex = groupIndex;
		}
	}

	public class CreativeContentGroup
	{
		public int Category { get; set; }
		public string Name { get; set; } = string.Empty;
		public NetworkItemInstanceDescriptor Icon { get; set; } = NetworkItemInstanceDescriptor.Empty;

		public CreativeContentGroup()
		{
		}

		public CreativeContentGroup(int category, string name, NetworkItemInstanceDescriptor icon)
		{
			Category = category;
			Name = name;
			Icon = icon;
		}
	}
}