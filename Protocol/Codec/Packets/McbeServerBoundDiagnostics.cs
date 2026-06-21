using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeServerBoundDiagnostics : Packet
{
    public McbeServerBoundDiagnostics()
    {
        Id = 315;
        IsMcbe = true;
    }

    public float AvgFps { get; set; }
    public float AvgServerSimTickTimeMS { get; set; }
    public float AvgClientSimTickTimeMS { get; set; }
    public float AvgBeginFrameTimeMS { get; set; }
    public float AvgInputTimeMS { get; set; }
    public float AvgRenderTimeMS { get; set; }
    public float AvgEndFrameTimeMS { get; set; }
    public float AvgRemainderTimePercent { get; set; }
    public float AvgUnaccountedTimePercent { get; set; }
    
    public List<MemoryCategoryCounter> MemoryCategoryValues { get; set; } = [];
    public List<EntityDiagnosticTimingInfo> EntityDiagnostics { get; set; } = [];
    public List<SystemDiagnosticTimingInfo> SystemDiagnostics { get; set; } = [];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(AvgFps);
        Write(AvgServerSimTickTimeMS);
        Write(AvgClientSimTickTimeMS);
        Write(AvgBeginFrameTimeMS);
        Write(AvgInputTimeMS);
        Write(AvgRenderTimeMS);
        Write(AvgEndFrameTimeMS);
        Write(AvgRemainderTimePercent);
                Write(AvgUnaccountedTimePercent);

                WriteSlice(MemoryCategoryValues.ToArray(), Write);
                WriteSlice(EntityDiagnostics.ToArray(), Write);
        WriteSlice(SystemDiagnostics.ToArray(), Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        AvgFps = ReadFloat();
        AvgServerSimTickTimeMS = ReadFloat();
        AvgClientSimTickTimeMS = ReadFloat();
        AvgBeginFrameTimeMS = ReadFloat();
        AvgInputTimeMS = ReadFloat();
        AvgRenderTimeMS = ReadFloat();
        AvgEndFrameTimeMS = ReadFloat();
        AvgRemainderTimePercent = ReadFloat();
        AvgUnaccountedTimePercent = ReadFloat();

        MemoryCategoryValues = ReadSlice(ReadMemoryCategoryCounter).ToList();
        EntityDiagnostics = ReadSlice(ReadEntityDiagnosticTimingInfo).ToList();
        SystemDiagnostics = ReadSlice(ReadSystemDiagnosticTimingInfo).ToList();
    }
}
