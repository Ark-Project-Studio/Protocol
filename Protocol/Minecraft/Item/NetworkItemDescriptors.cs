namespace Protocol.Minecraft;

public class NetworkItemStackDescriptor
{
	public int Id { get; set; }
	public ushort StackSize { get; set; }
	public uint Aux { get; set; }
	public Optional<int> NetId { get; set; } = new();
	public int BlockRuntimeId { get; set; }
	public string UserData { get; set; } = string.Empty;

	public static NetworkItemStackDescriptor Empty { get; } = new();
}

public class NetworkItemInstanceDescriptor
{
	public int Id { get; set; }
	public ushort StackSize { get; set; }
	public uint Aux { get; set; }
	public int BlockRuntimeId { get; set; }
	public string UserData { get; set; } = string.Empty;

	public static NetworkItemInstanceDescriptor Empty { get; } = new();
}

public enum RecipeIngredientDescriptorType : byte
{
	Invalid = 0,
	InternalItem = 1,
	Molang = 2,
	ItemTag = 3,
	Deferred = 4,
	ComplexAlias = 5
}

public class RecipeIngredient
{
	public RecipeIngredientDescriptorType DescriptorType { get; set; }
	public short Id { get; set; }
	public short Aux { get; set; }
	public string Name { get; set; } = string.Empty;
	public byte MolangVersion { get; set; }
	public int StackSize { get; set; }
}

public enum ItemVersion : int
{
	Legacy = 0,
	DataDriven = 1,
	None = 2
}
