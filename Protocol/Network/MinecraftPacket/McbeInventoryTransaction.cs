#define NeedComplete
using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventoryTransaction : Packet
{
    public enum CraftingAction
    {
        CraftAddIngredient = -2,
        CraftRemoveIngredient = -3,
        CraftResult = -4,
        CraftUseIngredient = -5,
        AnvilInput = -10,
        AnvilMaterial = -11,
        AnvilResult = -12,
        AnvilOutput = -13,
        EnchantItem = -15,
        EnchantLapis = -16,
        EnchantResult = -17,
        Drop = -100
    }

    public enum InventoryTransactionSourceType : int
    {
        InvalidInventory = -1,
        ContainerInventory = 0,
        GlobalInventory = 1,
        WorldInteraction = 2,
        CreativeInventory = 3,
        NonImplementedFeatureTODO = 99999
    }

    public enum ItemReleaseActionType : uint
    {
        Release = 0,
        Use = 1
    }

    public enum ItemUseActionType : uint
    {
        Place = 0,
        Use = 1,
        Destroy = 2,
        UseAsAttack = 3
    }

    public enum ItemUseOnActorActionType : uint
    {
        Interact = 0,
        Attack = 1,
        ItemInteract = 2
    }

    public enum InventoryTransactionType : uint
    {
        NormalTransaction = 0,
        InventoryMismatch = 1,
        ItemUseInventoryTransaction = 2,
        ItemUseOnActorInventoryTransaction = 3,
        ItemReleaseInventoryTransaction = 4
    }

    public enum TriggerType
    {
        Unknown = 0,
        PlayerInput = 1,
        SimulationTick = 2
    }

    public Transaction transaction;
    public McbeInventoryTransaction()
    {
        Id = 0x1e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(transaction);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        transaction = ReadTransaction();
    }
}
