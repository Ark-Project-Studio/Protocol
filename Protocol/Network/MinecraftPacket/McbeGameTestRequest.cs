using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public enum GameTestRequestRotation : byte
{
    Rotation0 = 0,
    Rotation90 = 1,
    Rotation180 = 2,
    Rotation270 = 3,
    Rotation360 = 4
}

public class McbeGameTestRequest : Packet
{
    public McbeGameTestRequest()
    {
        Id = 194;
        IsMcbe = true;
    }

    public string Name { get; set; } = string.Empty;
    public GameTestRequestRotation Rotation { get; set; }
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
        Rotation = (GameTestRequestRotation)ReadByte();
        StopOnError = ReadBool();
        Position = ReadBlockCoordinates();
        TestsPerRow = ReadSignedVarInt();
        Name = ReadString();
    }
}
