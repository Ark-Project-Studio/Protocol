using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeSetHealth : Packet
{
    public int health;
    public McbeSetHealth()
    {
        Id = 0x2a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(health);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        health = ReadSignedVarInt();
    }
}
