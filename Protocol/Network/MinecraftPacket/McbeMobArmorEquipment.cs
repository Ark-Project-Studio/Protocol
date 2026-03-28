using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeMobArmorEquipment : Packet
{
    public Item body;
    public Item boots;
    public Item chestplate;
    public Item helmet;
    public Item leggings;
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
        helmet = ReadItem();
        chestplate = ReadItem();
        leggings = ReadItem();
        boots = ReadItem();
        body = ReadItem();
    }
}
