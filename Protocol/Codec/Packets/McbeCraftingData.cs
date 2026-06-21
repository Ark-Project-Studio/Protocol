using Protocol.Minecraft.Inventory.Recipe;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeCraftingData : Packet
{
    public CraftingDataEntry[] craftingDataEntries = [];
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
