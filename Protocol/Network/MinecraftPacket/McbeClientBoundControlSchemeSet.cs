namespace Protocol.Network.MinecraftPacket;
public enum ControlSchemeType : byte
{
    LockedPlayerRelativeStrafe = 0,
    CameraRelative = 1,
    CameraRelativeStrafe = 2,
    PlayerRelative = 3,
    PlayerRelativeStrafe = 4
}

public class McbeClientBoundControlSchemeSet : Packet
{
    public McbeClientBoundControlSchemeSet()
    {
        Id = 327;
        IsMcbe = true;
    }

    public ControlSchemeType ControlScheme { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)ControlScheme);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ControlScheme = (ControlSchemeType)ReadByte();
    }
}
