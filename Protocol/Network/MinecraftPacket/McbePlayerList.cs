using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbePlayerList : Packet
{
    public PlayerRecords records;
    public McbePlayerList()
    {
        Id = 0x3f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(records);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        records = ReadPlayerRecords();
    }
}
