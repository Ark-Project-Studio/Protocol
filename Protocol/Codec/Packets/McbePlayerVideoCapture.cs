using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum PlayerVideoCaptureAction : byte
{
    Start = 0,
    Stop = 1
}

public class McbePlayerVideoCapture : Packet
{
    public McbePlayerVideoCapture()
    {
        Id = 324;
        IsMcbe = true;
    }

    public PlayerVideoCaptureAction Action { get; set; }
    public uint FrameRate { get; set; } = 30;
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
        FrameRate = 30;
        FilePrefix = string.Empty;
        if (Action == PlayerVideoCaptureAction.Start)
        {
            FrameRate = ReadUint();
            FilePrefix = ReadString();
        }
    }
}
