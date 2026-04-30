using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeBlockEvent : Packet
{
    public int eventType;
    public int eventValue;
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
        WriteSignedVarInt(eventType);
        WriteSignedVarInt(eventValue);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        eventType = ReadSignedVarInt();
        eventValue = ReadSignedVarInt();
    }
}
