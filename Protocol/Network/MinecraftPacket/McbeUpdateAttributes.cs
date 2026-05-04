using Protocol.Minecraft.Actor.Player;

namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateAttributes : Packet
{
    public PlayerAttributes attributes;
    public ulong runtimeEntityId;
    public ulong tick;
    public McbeUpdateAttributes()
    {
        Id = 0x1d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(attributes);
        WriteUnsignedVarLong(tick);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        attributes = ReadPlayerAttributes();
        tick = ReadUnsignedVarLong();
    }
}
