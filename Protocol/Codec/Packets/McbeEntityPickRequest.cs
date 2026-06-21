using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeEntityPickRequest : Packet
{
    public bool withData;
    public long actorId;
    public byte maxSlots;
    public McbeEntityPickRequest()
    {
        Id = 0x23;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(actorId);
        Write(maxSlots);
        Write(withData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        actorId = ReadLong();
        maxSlots = ReadByte();
        withData = ReadBool();
    }
}
