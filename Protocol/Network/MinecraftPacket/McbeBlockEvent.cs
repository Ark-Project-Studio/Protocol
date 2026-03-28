using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeBlockEvent : Packet
{
    public int case1;
    public int case2;
    public BlockCoordinates coordinates;
    public McbeBlockEvent()
    {
        Id = 0x1a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(coordinates);
        WriteSignedVarInt(case1);
        WriteSignedVarInt(case2);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        case1 = ReadSignedVarInt();
        case2 = ReadSignedVarInt();
    }
}
