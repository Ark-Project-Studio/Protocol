using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum LessonAction : int
{
    Start = 0,
    Complete = 1,
    Restart = 2
}

public class McbeLessonProgress : Packet
{
    public McbeLessonProgress()
    {
        Id = 183;
        IsMcbe = true;
    }

    public string Identifier { get; set; } = string.Empty;
    public LessonAction Action { get; set; }
    public int Score { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt((int)Action);
        WriteSignedVarInt(Score);
        Write(Identifier);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Action = (LessonAction)ReadSignedVarInt();
        Score = ReadSignedVarInt();
        Identifier = ReadString();
    }
}
