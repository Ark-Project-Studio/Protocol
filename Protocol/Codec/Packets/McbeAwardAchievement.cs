using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeAwardAchievement : Packet
{
    public McbeAwardAchievement()
    {
        Id = 309;
        IsMcbe = true;
    }

    public int AchievementID { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(AchievementID);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        AchievementID = ReadInt();
    }
}
