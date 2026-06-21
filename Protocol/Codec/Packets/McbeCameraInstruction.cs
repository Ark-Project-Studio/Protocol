using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeCameraInstruction : Packet
{
    public McbeCameraInstruction()
    {
        Id = 300;
        IsMcbe = true;
    }
    public CameraInstruction Instruction { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Instruction);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Instruction = ReadCameraInstruction();
    }

}
