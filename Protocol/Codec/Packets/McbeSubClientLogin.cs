using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeSubClientLogin : Packet
{
    public byte[] payload;

    public McbeSubClientLogin()
    {
        Id = 0x5e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteByteArray(payload);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        payload = ReadByteArray();
    }
}
