using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public enum CameraAimAssistPresetOperation : byte
{
    Set = 0,
    AddToExisting = 1
}

public class McbeCameraAimAssistPresets : Packet
{
    public McbeCameraAimAssistPresets()
    {
        Id = 320;
        IsMcbe = true;
    }

    public CameraAimAssistCategoryDefinition[] Categories { get; set; } = new CameraAimAssistCategoryDefinition[0];
    public CameraAimAssistPreset[] Presets { get; set; } = new CameraAimAssistPreset[0];
    public CameraAimAssistPresetOperation Operation { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)(Categories?.Length ?? 0));
        if (Categories != null)
            foreach (var category in Categories)
                Write(category);
        WriteUnsignedVarInt((uint)(Presets?.Length ?? 0));
        if (Presets != null)
            foreach (var preset in Presets)
                Write(preset);
        Write((byte)Operation);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var categoriesCount = ReadUnsignedVarInt();
        Categories = new CameraAimAssistCategoryDefinition[categoriesCount];
        for (var i = 0; i < categoriesCount; i++)
            Categories[i] = ReadCameraAimAssistCategoryDefinition();
        var presetsCount = ReadUnsignedVarInt();
        Presets = new CameraAimAssistPreset[presetsCount];
        for (var i = 0; i < presetsCount; i++)
            Presets[i] = ReadCameraAimAssistPreset();
        Operation = (CameraAimAssistPresetOperation)ReadByte();
    }
}
