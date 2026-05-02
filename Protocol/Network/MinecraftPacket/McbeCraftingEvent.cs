using Protocol.Minecraft;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class McbeCraftingEvent : Packet
{
    public enum RecipeTypes
    {
        Shapeless = 0,
        Shaped = 1,
        Furnace = 2,
        FurnaceData = 3,
        Multi = 4,
        ShulkerBox = 5,
        ChemistryShapeless = 6,
        ChemistryShaped = 7,
        SmithingTransform = 8,
        SmithingTrim = 9
    }

    public NetworkItemStackDescriptors input;
    public UUID recipeId;
    public int recipeType;
    public NetworkItemStackDescriptors result;
    public byte windowId;
    public McbeCraftingEvent()
    {
        Id = 0x35;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(windowId);
        WriteSignedVarInt(recipeType);
        Write(recipeId);
        Write(input);
        Write(result);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = ReadByte();
        recipeType = ReadSignedVarInt();
        recipeId = ReadUUID();
        input = ReadNetworkItemStackDescriptors();
        result = ReadNetworkItemStackDescriptors();
    }
}
