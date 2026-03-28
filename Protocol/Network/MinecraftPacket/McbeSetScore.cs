using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeSetScore : Packet
{
    public enum ChangeTypes
    {
        Player = 1,
        Entity = 2,
        FakePlayer = 3
    }

    public enum Types
    {
        Change = 0,
        Remove = 1
    }

    public ScoreEntries entries;
    public McbeSetScore()
    {
        Id = 0x6c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(entries);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entries = ReadScoreEntries();
    }
}
