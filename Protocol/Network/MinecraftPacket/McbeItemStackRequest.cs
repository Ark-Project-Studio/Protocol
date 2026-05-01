using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeItemStackRequest : Packet
{
    public enum Type : byte
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

    public ItemStackRequests requests;
    public McbeItemStackRequest()
    {
        Id = 0x93;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(requests);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        requests = ReadItemStackRequests();
    }
}
