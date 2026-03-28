using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeServerBoundDiagnostics : Packet
{
    public McbeServerBoundDiagnostics()
    {
        Id = 315;
        IsMcbe = true;
    }

    public float AverageFramesPerSecond { get; set; }
    public float AverageServerSimTickTime { get; set; }
    public float AverageClientSimTickTime { get; set; }
    public float AverageBeginFrameTime { get; set; }
    public float AverageInputTime { get; set; }
    public float AverageRenderTime { get; set; }
    public float AverageEndFrameTime { get; set; }
    public float AverageRemainderTimePercent { get; set; }
    public float AverageUnaccountedTimePercent { get; set; }
    
    public List<MemoryCategoryCounter> MemoryCategoryValues { get; set; }


	protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(AverageFramesPerSecond);
        Write(AverageServerSimTickTime);
        Write(AverageClientSimTickTime);
        Write(AverageBeginFrameTime);
        Write(AverageInputTime);
        Write(AverageRenderTime);
        Write(AverageEndFrameTime);
        Write(AverageRemainderTimePercent);
        Write(AverageUnaccountedTimePercent);
        WriteSlice(MemoryCategoryValues.ToArray(),Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        AverageFramesPerSecond = ReadFloat();
        AverageServerSimTickTime = ReadFloat();
        AverageClientSimTickTime = ReadFloat();
        AverageBeginFrameTime = ReadFloat();
        AverageInputTime = ReadFloat();
        AverageRenderTime = ReadFloat();
        AverageEndFrameTime = ReadFloat();
        AverageRemainderTimePercent = ReadFloat();
        AverageUnaccountedTimePercent = ReadFloat();
        MemoryCategoryValues = ReadSlice(ReadMemoryCategoryCounter).ToList();
    }
}
