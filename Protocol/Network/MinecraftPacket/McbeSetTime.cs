namespace Protocol.Network.MinecraftPacket;
public class McbeSetTime : Packet
{
    public int time;
    public McbeSetTime()
    {
        Id = 0x0a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(time);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        time = ReadSignedVarInt();
    }
}
