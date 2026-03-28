using Protocol.Utils;

namespace Protocol.Minecraft;

public class Recipes : List<Recipe>
{
}

public abstract class Recipe
{
	public UUID Id { get; set; } = new(Guid.NewGuid().ToString());
	public string Block { get; set; }
}

public class MultiRecipe : Recipe
{
	public int UniqueId { get; set; }
}

public class ShapelessRecipe : Recipe
{
	public ShapelessRecipe()
	{
		Input = new List<Item>();
		Result = new List<Item>();
	}

	public ShapelessRecipe(List<Item> result, List<Item> input, string block = null) : this()
	{
		Result = result;
		Input = input;
		Block = block;
	}

	public ShapelessRecipe(Item result, List<Item> input, string block = null) : this()
	{
		Result.Add(result);
		Input = input;
		Block = block;
	}

	public int UniqueId { get; set; }
	public List<Item> Input { get; private set; }
	public List<Item> Result { get; }
}

public class ShapedRecipe : Recipe
{
	public ShapedRecipe(int width, int height)
	{
		Width = width;
		Height = height;
		Input = new Item[Width * height];
		Result = new List<Item>();
	}

	public ShapedRecipe(int width, int height, Item result, Item[] input, string block = null) : this(width, height)
	{
		Result.Add(result);
		Input = input;
		Block = block;
	}

	public ShapedRecipe(int width, int height, List<Item> result, Item[] input, string block = null) : this(width,
		height)
	{
		Result = result;
		Input = input;
		Block = block;
	}

	public int UniqueId { get; set; }
	public int Width { get; set; }
	public int Height { get; set; }
	public Item[] Input { get; set; }
	public List<Item> Result { get; set; }
}

public class SmeltingRecipe : Recipe
{
	public SmeltingRecipe()
	{
	}

	public SmeltingRecipe(Item result, Item input, string block = null) : this()
	{
		Result = result;
		Input = input;
		Block = block;
	}

	public Item Input { get; set; }
	public Item Result { get; set; }
}

public class SmithingTransformRecipe : Recipe
{
	public SmithingTransformRecipe()
	{
	}

	public SmithingTransformRecipe(string recipeid, Item output, Item template, Item input, Item addition,
		string block) : this()
	{
		RecipeId = recipeid;
		Output = output;
		Template = template;
		Input = input;
		Addition = addition;
		Block = block;
	}

	public string RecipeId { get; set; }
	public int UniqueId { get; set; }
	public Item Template { get; set; }
	public Item Input { get; set; }
	public Item Addition { get; set; }
	public Item Output { get; set; }
}

public class SmithingTrimRecipe : Recipe
{
	public SmithingTrimRecipe()
	{
	}

	public SmithingTrimRecipe(string recipeid, Item output, Item template, Item input, Item addition, string block) :
		this()
	{
		RecipeId = recipeid;
		Output = output;
		Template = template;
		Input = input;
		Addition = addition;
		Block = block;
	}

	public string RecipeId { get; set; }
	public int UniqueId { get; set; }
	public Item Template { get; set; }
	public Item Input { get; set; }
	public Item Addition { get; set; }
	public Item Output { get; set; }
}

public class PotionContainerChangeRecipe
{
	public int Input { get; set; }
	public int Ingredient { get; set; }
	public int Output { get; set; }
}

public class PotionTypeRecipe
{
	public int Input { get; set; }
	public int InputMeta { get; set; }
	public int Ingredient { get; set; }
	public int IngredientMeta { get; set; }
	public int Output { get; set; }
	public int OutputMeta { get; set; }
}

public class MaterialReducerRecipe
{
	public MaterialReducerRecipe()
	{
	}

	public MaterialReducerRecipe(int inputId, int inputMeta, params MaterialReducerRecipeOutput[] outputs)
	{
		Input = inputId;
		InputMeta = inputMeta;

		Output = outputs;
	}

	public int Input { get; set; }
	public int InputMeta { get; set; }

	public MaterialReducerRecipeOutput[] Output { get; set; }

	public class MaterialReducerRecipeOutput
	{
		public MaterialReducerRecipeOutput()
		{
		}

		public MaterialReducerRecipeOutput(int itemId, int itemCount)
		{
			ItemId = itemId;
			ItemCount = itemCount;
		}

		public int ItemId { get; set; }
		public int ItemCount { get; set; }
	}
}