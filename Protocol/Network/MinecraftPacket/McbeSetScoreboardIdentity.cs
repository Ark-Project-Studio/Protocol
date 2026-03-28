using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeSetScoreboardIdentity : Packet
{
    public enum Operations
    {
        RegisterIdentity = 0,
        ClearIdentity = 1
    }

    public ScoreboardIdentityEntries entries;
    public McbeSetScoreboardIdentity()
    {
        Id = 0x70;
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
        entries = ReadScoreboardIdentityEntries();
    }
}
