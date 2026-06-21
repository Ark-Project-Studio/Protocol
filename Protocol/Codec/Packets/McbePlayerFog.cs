using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbePlayerFog : Packet
{
    public fogStack fogstack;
    public McbePlayerFog()
    {
        Id = 0xa0;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(fogstack);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        fogstack = Read();
    }
}
