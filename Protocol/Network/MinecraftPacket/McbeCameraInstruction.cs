using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeCameraInstruction : Packet
{
    public McbeCameraInstruction()
    {
        Id = 300;
        IsMcbe = true;
    }

    public Optional<CameraAimAssistCategory> Set { get; set; }
    public Optional<bool> Clear { get; set; }
    public Optional<CameraAimAssistPreset> Fade { get; set; }
    public Optional<CameraAimAssistItemSettings> Target { get; set; }
    public Optional<bool> RemoveTarget { get; set; }
    public Optional<CameraAimAssistPriorities> FieldOfView { get; set; }
    public Optional<CameraSplineInstruction> Spline { get; set; }
    public Optional<long> AttachToEntity { get; set; }
    public Optional<bool> DetachFromEntity { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Set.HasValue);
        if (Set.HasValue)
            Write(Set.Value);
        Write(Clear.HasValue);
        if (Clear.HasValue)
            Write(Clear.Value);
        Write(Fade.HasValue);
        if (Fade.HasValue)
            Write(Fade.Value);
        Write(Target.HasValue);
        if (Target.HasValue)
            Write(Target.Value);
        Write(RemoveTarget.HasValue);
        if (RemoveTarget.HasValue)
            Write(RemoveTarget.Value);
        Write(FieldOfView.HasValue);
        if (FieldOfView.HasValue)
            Write(FieldOfView.Value);
        if (Spline.HasValue)
        {
            Write(Spline.Value);
        }

        if (AttachToEntity.HasValue)
        {
            Write(AttachToEntity.Value);
        }

        if (DetachFromEntity.HasValue)
        {
            Write(DetachFromEntity.Value);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var hasSet = ReadBool();
        if (hasSet)
            Set = new Optional<CameraAimAssistCategory>(ReadCameraAimAssistCategory());
        var hasClear = ReadBool();
        if (hasClear)
            Clear = new Optional<bool>(ReadBool());
        var hasFade = ReadBool();
        if (hasFade)
            Fade = new Optional<CameraAimAssistPreset>(ReadCameraAimAssistPreset());
        var hasTarget = ReadBool();
        if (hasTarget)
            Target = new Optional<CameraAimAssistItemSettings>(ReadCameraAimAssistItemSettings());
        var hasRemoveTarget = ReadBool();
        if (hasRemoveTarget)
            RemoveTarget = new Optional<bool>(ReadBool());
        var hasFieldOfView = ReadBool();
        if (hasFieldOfView)
            FieldOfView = new Optional<CameraAimAssistPriorities>(ReadCameraAimAssistPriorities());
        var hasFieldOfCamera = ReadBool();
        if (hasFieldOfCamera)
        {
            Spline = new Optional<CameraSplineInstruction>(ReadCameraSplineInstruction());
        }

        if (ReadBool())
        {
            AttachToEntity = new Optional<long>(ReadLong());
        }

        if (ReadBool())
        {
            DetachFromEntity = new Optional<bool>(ReadBool());
        }
    }

}
