using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeMoveEntity : Packet
{
    public byte flags;
    public PlayerLocation position;
    public ulong runtimeEntityId;
    public McbeMoveEntity()
    {
        Id = 0x12;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(flags);
        Write(position);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        flags = ReadByte();
        position = ReadPlayerLocation();
    }
}
