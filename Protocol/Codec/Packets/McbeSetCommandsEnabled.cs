using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeSetCommandsEnabled : Packet
{
    public bool enabled;
    public McbeSetCommandsEnabled()
    {
        Id = 0x3b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(enabled);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        enabled = ReadBool();
    }
}
