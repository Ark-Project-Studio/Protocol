using Protocol.Utils;

namespace Protocol.Minecraft;

public class CraftingDataEntry
{
	public CraftingDataEntryType Type { get; set; }
	public Recipe Recipe { get; set; } = new ShapelessRecipe();
}

public enum CraftingDataEntryType : int
{
	Shapeless = 0,
	Shaped = 1,
	Furnace = 2,
	FurnaceAux = 3,
	Multi = 4,
	UserDataShapeless = 5,
	ShapelessChemistry = 6,
	ShapedChemistry = 7,
	SmithingTransform = 8,
	SmithingTrim = 9
}

public abstract class Recipe
{
}

public enum RecipeUnlockingContext : byte
{
	None = 0,
	AlwaysUnlocked = 1,
	PlayerInWater = 2,
	PlayerHasManyItems = 3
}

public class RecipeUnlockingRequirement
{
	public RecipeUnlockingContext UnlockingContext { get; set; }
	public List<RecipeIngredient> UnlockingIngredients { get; set; } = new();
}

public class ShapelessRecipe : Recipe
{
	public string RecipeUniqueId { get; set; } = string.Empty;
	public List<RecipeIngredient> IngredientList { get; set; } = new();
	public NetworkItemInstanceDescriptor[] ProductionList { get; set; } = [];
	public UUID RecipeId { get; set; } = new(Guid.Empty.ToString());
	public string RecipeTag { get; set; } = string.Empty;
	public int Priority { get; set; }
	public RecipeUnlockingRequirement UnlockingRequirement { get; set; } = new();
	public uint NetId { get; set; }
}

public class ShapedRecipe : Recipe
{
	public string RecipeUniqueId { get; set; } = string.Empty;
	public int GridWidth { get; set; }
	public int GridHeight { get; set; }
	public List<RecipeIngredient> IngredientList { get; set; } = new();
	public NetworkItemInstanceDescriptor[] ProductionList { get; set; } = [];
	public UUID RecipeId { get; set; } = new(Guid.Empty.ToString());
	public string RecipeTag { get; set; } = string.Empty;
	public int Priority { get; set; }
	public bool AssumeSymmetry { get; set; }
	public RecipeUnlockingRequirement UnlockingRequirement { get; set; } = new();
	public uint NetId { get; set; }
}

public class FurnaceRecipe : Recipe
{
}

public class FurnaceAuxRecipe : Recipe
{
}

public class MultiRecipe : Recipe
{
	public UUID MultiRecipeId { get; set; } = new(Guid.Empty.ToString());
	public uint NetId { get; set; }
}

public class UserDataShapelessRecipe : Recipe
{
	public string RecipeUniqueId { get; set; } = string.Empty;
	public List<RecipeIngredient> IngredientList { get; set; } = new();
	public NetworkItemInstanceDescriptor[] ProductionList { get; set; } = [];
	public UUID RecipeId { get; set; } = new(Guid.Empty.ToString());
	public string RecipeTag { get; set; } = string.Empty;
	public int Priority { get; set; }
	public RecipeUnlockingRequirement UnlockingRequirement { get; set; } = new();
	public uint NetId { get; set; }
}

public class ShapelessChemistryRecipe : Recipe
{
	public string RecipeUniqueId { get; set; } = string.Empty;
	public List<RecipeIngredient> IngredientList { get; set; } = new();
	public NetworkItemInstanceDescriptor[] ProductionList { get; set; } = [];
	public UUID RecipeId { get; set; } = new(Guid.Empty.ToString());
	public string RecipeTag { get; set; } = string.Empty;
	public int Priority { get; set; }
	public uint NetId { get; set; }
}

public class ShapedChemistryRecipe : Recipe
{
	public ShapedRecipe ChemistryRecipe { get; set; } = new();
}

public class SmithingTransformRecipe : Recipe
{
	public string RecipeUniqueId { get; set; } = string.Empty;
	public RecipeIngredient Template { get; set; } = new();
	public RecipeIngredient Base { get; set; } = new();
	public RecipeIngredient Addition { get; set; } = new();
	public NetworkItemInstanceDescriptor Result { get; set; } = NetworkItemInstanceDescriptor.Empty;
	public string RecipeTag { get; set; } = string.Empty;
	public uint NetId { get; set; }
}

public class SmithingTrimRecipe : Recipe
{
	public string RecipeUniqueId { get; set; } = string.Empty;
	public RecipeIngredient Template { get; set; } = new();
	public RecipeIngredient Base { get; set; } = new();
	public RecipeIngredient Addition { get; set; } = new();
	public string RecipeTag { get; set; } = string.Empty;
	public uint NetId { get; set; }
}

public class PotionMixDataEntry
{
	public int FromPotionId { get; set; }
	public int FromPotionAux { get; set; }
	public int ReagentItemId { get; set; }
	public int ReagentItemAux { get; set; }
	public int ToPotionId { get; set; }
	public int ToPotionAux { get; set; }
}

public class ContainerMixDataEntry
{
	public int InputItemId { get; set; }
	public int ReagentItemId { get; set; }
	public int ToItemId { get; set; }
}

public class MaterialReducerDataEntry
{
	public int Input { get; set; }
	public List<MaterialReducerItemInfo> Items { get; set; } = new();
}

public class MaterialReducerItemInfo
{
	public int ItemId { get; set; }
	public int ItemCount { get; set; }
}