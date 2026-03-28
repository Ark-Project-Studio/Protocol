using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeItemStackRequest : Packet
{
    public enum ActionType
    {
        Take = 0,
        Place = 1,
        Swap = 2,
        Drop = 3,
        Destroy = 4,
        Consume = 5,
        Create = 6,
        PlaceIntoBundleDeprecated = 7,
        TakeFromBundleDeprecated = 8,
        LabTableCombine = 9,
        BeaconPayment = 10,
        MineBlock = 11,
        CraftRecipe = 12,
        CraftRecipeAuto = 13,
        CraftCreative = 14,
        CraftRecipeOptional = 15,
        CraftGrindstone = 16,
        CraftLoom = 17,
        CraftNotImplementedDeprecated = 18,
        CraftResultsDeprecated = 19
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
