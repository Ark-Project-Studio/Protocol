using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets;
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

    public enum ClientCooldownState : byte
    {
        Off = 0,
        On = 1
    }

    public enum PredictedResult : byte
    {
        Failure = 0,
        Success = 1
    }

    public int LegacyRequestRawId { get; set; }
    public List<InventoryTransactionLegacySetItemSlot> LegacySetItemSlots { get; set; } = new();
    public List<InventoryTransactionAction> InventoryTransactionActions { get; set; } = new();
    public InventoryTransactionType TransactionType { get; set; } = InventoryTransactionType.NormalTransaction;
    public InventoryTransactionData TransactionData { get; set; } = new InventoryNormalTransactionData();

    public McbeInventoryTransaction()
    {
        Id = 0x1e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteInventoryTransactionPacket(this);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ReadInventoryTransactionPacket(this);
    }
}

public class InventoryTransactionAction
{
    public McbeInventoryTransaction.InventoryTransactionSourceType SourceType { get; set; }
    public int ContainerId { get; set; }
    public uint BitFlags { get; set; }
    public uint InventorySlot { get; set; }
    public NetworkItemStackDescriptor FromItem { get; set; } = NetworkItemStackDescriptor.Empty;
    public NetworkItemStackDescriptor ToItem { get; set; } = NetworkItemStackDescriptor.Empty;
}

public class InventoryTransactionLegacySetItemSlot
{
    public byte ContainerId { get; set; }
    public List<byte> Slots { get; set; } = new();
}

public abstract class InventoryTransactionData
{
}

public sealed class InventoryNormalTransactionData : InventoryTransactionData
{
}

public sealed class InventoryMismatchTransactionData : InventoryTransactionData
{
}

public sealed class ItemUseInventoryTransactionData : InventoryTransactionData
{
    public McbeInventoryTransaction.ItemUseActionType ActionType { get; set; }
    public McbeInventoryTransaction.TriggerType TriggerType { get; set; }
    public BlockCoordinates Position { get; set; }
    public uint TargetBlockRuntimeId { get; set; }
    public int Face { get; set; }
    public int Slot { get; set; }
    public NetworkItemStackDescriptor Item { get; set; } = NetworkItemStackDescriptor.Empty;
    public System.Numerics.Vector3 FromPosition { get; set; }
    public System.Numerics.Vector3 ClickPosition { get; set; }
    public McbeInventoryTransaction.PredictedResult ClientPredictedResult { get; set; }
    public McbeInventoryTransaction.ClientCooldownState ClientCooldownState { get; set; }
}

public sealed class ItemUseOnActorInventoryTransactionData : InventoryTransactionData
{
    public ulong RuntimeId { get; set; }
    public McbeInventoryTransaction.ItemUseOnActorActionType ActionType { get; set; }
    public int Slot { get; set; }
    public NetworkItemStackDescriptor Item { get; set; } = NetworkItemStackDescriptor.Empty;
    public System.Numerics.Vector3 FromPosition { get; set; }
    public System.Numerics.Vector3 HitPosition { get; set; }
}

public sealed class ItemReleaseInventoryTransactionData : InventoryTransactionData
{
    public McbeInventoryTransaction.ItemReleaseActionType ActionType { get; set; }
    public int Slot { get; set; }
    public NetworkItemStackDescriptor Item { get; set; } = NetworkItemStackDescriptor.Empty;
    public System.Numerics.Vector3 FromPosition { get; set; }
}
