using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeMobEquipment : Packet
{
    /// <summary>
    /// Runtime ID of the mob or player whose equipment is changing.
    /// </summary>
    public ulong runtimeEntityId;

    /// <summary>
    /// The item stack being equipped. Empty item indicates no item.
    /// </summary>
    public NetworkItemStackDescriptor item;

    /// <summary>
    /// The inventory slot index the item occupies within the container.
    /// </summary>
    public byte slot;

    /// <summary>
    /// The hotbar slot that is currently selected by the player.
    /// Matches slot for non-player mobs.
    /// </summary>
    public byte selectedSlot;

    /// <summary>
    /// Identifies which container the item belongs to, e.g. inventory or offhand.
    /// </summary>
    public byte containerId;

    public McbeMobEquipment()
    {
        Id = 0x1f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(item);
        Write(slot);
        Write(selectedSlot);
        Write(containerId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        ReadNetworkItemStackDescriptor();
        slot = ReadByte();
        selectedSlot = ReadByte();
        containerId = ReadByte();
    }
}
