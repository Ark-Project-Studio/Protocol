using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class ScopeDataSummary
{
    public string Label { get; set; } = string.Empty;
    public string Indentation { get; set; } = string.Empty;
    public ulong TotalHighCostNS { get; set; }
    public ulong TotalMidCostNS { get; set; }
    public ulong TotalLowCostNS { get; set; }
}

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
    public List<ScopeDataSummary> ScopeDataSummaries { get; set; } = [];

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
        WriteSlice(ScopeDataSummaries.ToArray(), WriteScopeDataSummary);
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
        ScopeDataSummaries = ReadSlice(ReadScopeDataSummary).ToList();
    }

    private void WriteScopeDataSummary(ScopeDataSummary summary)
    {
        Write(summary.Label);
        Write(summary.Indentation);
        Write(summary.TotalHighCostNS);
        Write(summary.TotalMidCostNS);
        Write(summary.TotalLowCostNS);
    }

    private ScopeDataSummary ReadScopeDataSummary()
    {
        return new ScopeDataSummary
        {
            Label = ReadString(),
            Indentation = ReadString(),
            TotalHighCostNS = ReadUlong(),
            TotalMidCostNS = ReadUlong(),
            TotalLowCostNS = ReadUlong()
        };
    }
}
