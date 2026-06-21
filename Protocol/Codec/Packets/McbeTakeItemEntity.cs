using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeTakeItemEntity : Packet
{
    public ulong runtimeEntityId;
    public ulong target;
    public McbeTakeItemEntity()
    {
        Id = 0x11;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        WriteUnsignedVarLong(target);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        target = ReadUnsignedVarLong();
    }
}
