using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbePlaySound : Packet
{
    public BlockCoordinates coordinates;
    public string name;
    public float pitch;
    public float volume;
    public McbePlaySound()
    {
        Id = 0x56;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(name);
        Write(coordinates);
        Write(volume);
        Write(pitch);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        name = ReadString();
        coordinates = ReadBlockCoordinates();
        volume = ReadFloat();
        pitch = ReadFloat();
    }
}
