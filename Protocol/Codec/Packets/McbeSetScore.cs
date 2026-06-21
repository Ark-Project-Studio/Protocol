#define Needed
using Protocol.Minecraft.Level.Scoreboard;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeSetScore : Packet
{
    public enum IdentityType : byte
    {
        Invalid = 0,
        Player = 1,
        Entity = 2,
        FakePlayer = 3
    }

    public enum PacketType : byte
    {
        Change = 0,
        Remove = 1
    }

    public ScoreEntry[] entries;
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
