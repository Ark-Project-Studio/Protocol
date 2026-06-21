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
        WriteCereal(helmet);
        WriteCereal(chestplate);
        WriteCereal(leggings);
        WriteCereal(boots);
        WriteCereal(body);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        helmet = ReadCerealNetworkItemStackDescriptor();
        chestplate = ReadCerealNetworkItemStackDescriptor();
        leggings = ReadCerealNetworkItemStackDescriptor();
        boots = ReadCerealNetworkItemStackDescriptor();
        body = ReadCerealNetworkItemStackDescriptor();
    }
}
