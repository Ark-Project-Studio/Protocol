using fNbt;


namespace Protocol.Minecraft
{
	public class creativeGroup
	{
		public int Category { get; set; }
		public string Name { get; set; }
		public Item Icon { get; set; }

		public creativeGroup(int category, string name, Item icon)
		{
			Category = category;
			Name = name;
			Icon = icon;
		}
	}

	public class CreativeItemEntry
	{
		public uint GroupIndex { get; set; }
		public Item Item { get; set; }

		public CreativeItemEntry(uint groupIndex, Item item)
		{
			GroupIndex = groupIndex;
			Item = item;
		}
	}
}