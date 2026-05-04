using Protocol.Minecraft.Inventory.Item;

namespace Protocol.Minecraft.Inventory.Transaction;

public enum ItemStackRequestActionType : byte
{
	Take = 0,
	Place = 1,
	Swap = 2,
	Drop = 3,
	Destroy = 4,
	Consume = 5,
	Create = 6,
	PlaceInItemContainerDeprecated = 7,
	TakeFromItemContainerDeprecated = 8,
	LabTableCombine = 9,
	BeaconPayment = 10,
	MineBlock = 11,
	CraftRecipe = 12,
	CraftRecipeAuto = 13,
	CraftCreative = 14,
	CraftRecipeOptional = 15,
	CraftGrindStone = 16,
	CraftLoom = 17,
	CraftNonImplemented = 18,
	CraftResults = 19
}

public enum MineBlockPreValidationStatus : byte
{
	Valid = 0,
	Invalid = 1
}

public class ItemStackAction
{
	public ItemStackRequestActionType ActionType { get; set; }
	public byte Amount { get; set; }
	public StackRequestSlotInfo Source { get; set; } = new();
	public StackRequestSlotInfo Destination { get; set; } = new();
	public bool Randomly { get; set; }
	public byte ResultsIndex { get; set; }
	public int PrimaryEffectId { get; set; }
	public int SecondaryEffectId { get; set; }
	public int Slot { get; set; }
	public int PredictedDurability { get; set; }
	public int ItemStackNetId { get; set; }
	public MineBlockPreValidationStatus PreValidationStatus { get; set; }
	public uint RecipeNetworkIdOrCreativeId { get; set; }
	public uint TimesCraftedVarInt { get; set; }
	public byte TimesCrafted { get; set; }
	public byte NumberOfRequestedCrafts { get; set; }
	public List<RecipeIngredient> Ingredients { get; set; } = new();
	public uint RecipeNetId { get; set; }
	public uint FilteredStringIndex { get; set; }
	public int RepairCost { get; set; }
	public string PatternNameId { get; set; } = string.Empty;
	public NetworkItemInstanceDescriptor[] CraftResults { get; set; } = [];
}