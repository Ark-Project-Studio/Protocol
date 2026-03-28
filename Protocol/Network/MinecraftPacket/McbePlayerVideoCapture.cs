namespace Protocol.Network.MinecraftPacket;
public enum PlayerVideoCaptureAction : byte
{
    Stop = 0,
    Start = 1
}

public class McbePlayerVideoCapture : Packet
{
    public McbePlayerVideoCapture()
    {
        Id = 324;
        IsMcbe = true;
    }

    public PlayerVideoCaptureAction Action { get; set; }
    public int FrameRate { get; set; }
    public string FilePrefix { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)Action);
        if (Action == PlayerVideoCaptureAction.Start)
        {
            Write(FrameRate);
            Write(FilePrefix);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Action = (PlayerVideoCaptureAction)ReadByte();
        FrameRate = 0;
        FilePrefix = string.Empty;
        if (Action == PlayerVideoCaptureAction.Start)
        {
            FrameRate = ReadInt();
            FilePrefix = ReadString();
        }
    }
}
