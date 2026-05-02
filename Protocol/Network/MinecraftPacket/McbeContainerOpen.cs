using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public enum ContainerID : byte
{
    Inventory = 0,
    First = 1,
    Last = 100,
    Offhand = 119,
    Armor = 120,
    SelectionSlots = 122,
    PlayerOnlyUi = 124,
    Registry = 125,
    None = 255
}

public enum ContainerType : byte
{
    Container = 0,
    Workbench = 1,
    Furnace = 2,
    Enchantment = 3,
    BrewingStand = 4,
    Anvil = 5,
    Dispenser = 6,
    Dropper = 7,
    Hopper = 8,
    Cauldron = 9,
    MinecartChest = 10,
    MinecartHopper = 11,
    Horse = 12,
    Beacon = 13,
    StructureEditor = 14,
    Trade = 15,
    CommandBlock = 16,
    Jukebox = 17,
    Armor = 18,
    Hand = 19,
    CompoundCreator = 20,
    ElementConstructor = 21,
    MaterialReducer = 22,
    LabTable = 23,
    Loom = 24,
    Lectern = 25,
    Grindstone = 26,
    BlastFurnace = 27,
    Smoker = 28,
    Stonecutter = 29,
    Cartography = 30,
    Hud = 31,
    JigsawEditor = 32,
    SmithingTable = 33,
    ChestBoat = 34,
    DecoratedPot = 35,
    Crafter = 36,
    None = 247,
    Inventory = 255
}

public class McbeContainerOpen : Packet
{
    public BlockCoordinates coordinates;
    public ulong runtimeEntityId;
    public ContainerType type;
    public ContainerID windowId;
    public McbeContainerOpen()
    {
        Id = 0x2e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)windowId);
        Write((byte)type);
        Write(coordinates);
        WriteSignedVarLong((long)runtimeEntityId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = (ContainerID)ReadByte();
        type = (ContainerType)ReadByte();
        coordinates = ReadBlockCoordinates();
        runtimeEntityId = (ulong)ReadSignedVarLong();
    }
}
