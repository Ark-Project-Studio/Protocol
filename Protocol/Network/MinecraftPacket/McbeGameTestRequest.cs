using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public enum Rotation : byte
{
    None = 0,
    Rotate90 = 1,
    Rotate180 = 2,
    Rotate270 = 3,
    Clockwise90 = 1,
    Clockwise180 = 2,
    CounterClockwise90 = 3
}

public class McbeGameTestRequest : Packet
{
    public McbeGameTestRequest()
    {
        Id = 194;
        IsMcbe = true;
    }

    public string Name { get; set; } = string.Empty;
    public Rotation Rotation { get; set; }
    public int Repetitions { get; set; }
    public BlockCoordinates Position { get; set; }
    public bool StopOnError { get; set; }
    public int TestsPerRow { get; set; }
    public int MaxTestsPerBatch { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(MaxTestsPerBatch);
        WriteSignedVarInt(Repetitions);
        Write((byte)Rotation);
        Write(StopOnError);
        Write(Position);
        WriteSignedVarInt(TestsPerRow);
        Write(Name);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        MaxTestsPerBatch = ReadSignedVarInt();
        Repetitions = ReadSignedVarInt();
        Rotation = (Rotation)ReadByte();
        StopOnError = ReadBool();
        Position = ReadBlockCoordinates();
        TestsPerRow = ReadSignedVarInt();
        Name = ReadString();
    }
}
