using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbePlaySound : Packet
{
    public BlockCoordinates coordinates;
    public string name;
    public float pitch;
    public Optional<bool> serverSoundHandle;
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
        Write(serverSoundHandle.HasValue);
        if (serverSoundHandle.HasValue)
        {
            Write(serverSoundHandle.Value);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        name = ReadString();
        coordinates = ReadBlockCoordinates();
        volume = ReadFloat();
        pitch = ReadFloat();
        bool hasServerSoundHandle = ReadBool();
        if (hasServerSoundHandle)
        {
            serverSoundHandle = new Optional<bool>(ReadBool());
        }
    }
}
