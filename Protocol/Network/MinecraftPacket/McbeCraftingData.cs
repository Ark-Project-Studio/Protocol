using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeCraftingData : Packet
{
    public bool isClean;
    public MaterialReducerRecipe[] materialReducerRecipes;
    public PotionContainerChangeRecipe[] potionContainerRecipes;
    public PotionTypeRecipe[] potionTypeRecipes;
    public Recipes recipes;
    public McbeCraftingData()
    {
        Id = 0x34;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(recipes);
        Write(potionTypeRecipes);
        Write(potionContainerRecipes);
        Write(materialReducerRecipes);
        Write(isClean);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        recipes = ReadRecipes();
        potionTypeRecipes = ReadPotionTypeRecipes();
        potionContainerRecipes = ReadPotionContainerChangeRecipes();
        materialReducerRecipes = ReadMaterialReducerRecipes();
        isClean = ReadBool();
    }
}
