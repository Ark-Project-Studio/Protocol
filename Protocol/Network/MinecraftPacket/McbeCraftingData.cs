using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeCraftingData : Packet
{
    public CraftingDataEntries craftingDataEntries = new();
    public List<PotionMixDataEntry> potionMixDataEntries = new();
    public List<ContainerMixDataEntry> containerMixDataEntries = new();
    public List<MaterialReducerDataEntry> materialReducerDataEntries = new();
    public bool clearRecipe;

    public McbeCraftingData()
    {
        Id = 0x34;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(craftingDataEntries);
        Write(potionMixDataEntries);
        Write(containerMixDataEntries);
        Write(materialReducerDataEntries);
        Write(clearRecipe);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        craftingDataEntries = ReadCraftingDataEntries();
        potionMixDataEntries = ReadPotionMixDataEntries();
        containerMixDataEntries = ReadContainerMixDataEntries();
        materialReducerDataEntries = ReadMaterialReducerDataEntries();
        clearRecipe = ReadBool();
    }
}
