namespace Protocol.Network.MinecraftPacket;
public enum ClientCameraAimAssistAction : byte
{
    SetFromCameraPreset = 0,
    Clear = 1
}

public class McbeClientCameraAimAssist : Packet
{
    public McbeClientCameraAimAssist()
    {
        Id = 321;
        IsMcbe = true;
    }

    public string PresetID { get; set; } = string.Empty;
    public ClientCameraAimAssistAction Action { get; set; }
    public bool AllowAimAssist { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(PresetID);
        Write((byte)Action);
        Write(AllowAimAssist);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        PresetID = ReadString();
        Action = (ClientCameraAimAssistAction)ReadByte();
        AllowAimAssist = ReadBool();
    }
}
