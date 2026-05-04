using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
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
