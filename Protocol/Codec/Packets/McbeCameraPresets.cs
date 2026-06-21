using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets;
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
        WriteSlice(Presets, Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Presets = ReadSlice(ReadCameraPreset);
    }
}
