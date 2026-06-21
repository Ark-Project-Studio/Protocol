using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeTickingAreasLoadStatus : Packet
{
    public McbeTickingAreasLoadStatus()
    {
        Id = 179;
        IsMcbe = true;
    }

    public bool Preload { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Preload);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Preload = ReadBool();
    }
}
