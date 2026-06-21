using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeRemoveEntity : Packet
{
    public long entityIdSelf;
    public McbeRemoveEntity()
    {
        Id = 0x0e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(entityIdSelf);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entityIdSelf = ReadSignedVarLong();
    }
}
