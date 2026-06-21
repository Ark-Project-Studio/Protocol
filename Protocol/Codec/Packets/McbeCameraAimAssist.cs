using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public enum CameraAimAssistAction : byte
{
    Set = 0,
    Clear = 1
}

public class McbeCameraAimAssist : Packet
{
    public McbeCameraAimAssist()
    {
        Id = 316;
        IsMcbe = true;
    }

    public string Preset { get; set; } = string.Empty;
    public Vector2 Angle { get; set; }
    public float Distance { get; set; }
    public Minecraft.AimAssistTargetMode TargetMode { get; set; }
    public CameraAimAssistAction Action { get; set; }
    public bool ShowDebugRender { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Preset);
        Write(Angle);
        Write(Distance);
        Write((byte)TargetMode);
        Write((byte)Action);
        Write(ShowDebugRender);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Preset = ReadString();
        Angle = ReadVector2();
        Distance = ReadFloat();
        TargetMode = (Minecraft.AimAssistTargetMode)ReadByte();
        Action = (CameraAimAssistAction)ReadByte();
        ShowDebugRender = ReadBool();
    }
}
