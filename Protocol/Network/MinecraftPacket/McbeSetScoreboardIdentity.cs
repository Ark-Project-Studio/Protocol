using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeSetScoreboardIdentity : Packet
{
    public enum Type : byte
    {
        Update = 0,
        Remove = 1
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
