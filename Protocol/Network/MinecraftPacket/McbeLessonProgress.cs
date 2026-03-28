namespace Protocol.Network.MinecraftPacket;
public enum LessonAction : byte
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
        Write((byte)Action);
        WriteSignedVarInt(Score);
        Write(Identifier);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Action = (LessonAction)ReadByte();
        Score = ReadSignedVarInt();
        Identifier = ReadString();
    }
}
