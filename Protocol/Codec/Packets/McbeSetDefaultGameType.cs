using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeSetDefaultGameType : Packet
{
    public int gamemode;
    public McbeSetDefaultGameType()
    {
        Id = 0x69;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteVarInt(gamemode);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        gamemode = ReadVarInt();
    }
}
