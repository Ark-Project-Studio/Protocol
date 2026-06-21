using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeLogin : Packet
{
    public byte[] payload;
    public int protocolVersion;
    public McbeLogin()
    {
        Id = 0x01;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteBe(protocolVersion);
        WriteByteArray(payload);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        protocolVersion = ReadIntBe();
        payload = ReadByteArray();
    }
}
