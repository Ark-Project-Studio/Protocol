using Protocol.Minecraft.Inventory.Item;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeMobArmorEquipment : Packet
{
    public NetworkItemStackDescriptor body;
    public NetworkItemStackDescriptor boots;
    public NetworkItemStackDescriptor chestplate;
    public NetworkItemStackDescriptor helmet;
    public NetworkItemStackDescriptor leggings;
    public ulong runtimeEntityId;
    public McbeMobArmorEquipment()
    {
        Id = 0x20;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(helmet);
        Write(chestplate);
        Write(leggings);
        Write(boots);
        Write(body);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        helmet = ReadNetworkItemStackDescriptor();
        chestplate = ReadNetworkItemStackDescriptor();
        leggings = ReadNetworkItemStackDescriptor();
        boots = ReadNetworkItemStackDescriptor();
        body = ReadNetworkItemStackDescriptor();
    }
}
