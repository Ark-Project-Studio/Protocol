using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeContainerSetData : Packet
{
    public int property;
    public int value;
    public ContainerID windowId;
    public McbeContainerSetData()
    {
        Id = 0x33;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)windowId);
        WriteSignedVarInt(property);
        WriteSignedVarInt(value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = (ContainerID)ReadByte();
        property = ReadSignedVarInt();
        value = ReadSignedVarInt();
    }
}
