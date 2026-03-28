namespace Protocol.Network.MinecraftPacket;
public class McbeCameraPresets : Packet
{
    public McbeCameraPresets()
    {
        Id = 198;
        IsMcbe = true;
    }

    public CameraPreset[] Presets { get; set; } = new CameraPreset[0];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)(Presets?.Length ?? 0));
        if (Presets != null)
            foreach (var preset in Presets)
                WriteCameraPreset(preset);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var count = ReadUnsignedVarInt();
        Presets = new CameraPreset[count];
        for (var i = 0; i < count; i++)
            Presets[i] = ReadCameraPreset();
    }

#region 琛ュ叏鐨勬柟娉?(鍥犱负 methods.txt 涓病鏈?
    private void WriteCameraPreset(CameraPreset preset)
    {
        if (preset == null)
        {
            Write(string.Empty);
            Write(string.Empty);
            return;
        }

        Write(preset.Name ?? string.Empty);
        Write(preset.Parent ?? string.Empty);
    }

    private CameraPreset ReadCameraPreset()
    {
        var preset = new CameraPreset();
        preset.Name = ReadString();
        preset.Parent = ReadString();
        return preset;
    }
#endregion
}

public class CameraPreset
{
    public string Name { get; set; } = string.Empty;
    public string Parent { get; set; } = string.Empty;
}
