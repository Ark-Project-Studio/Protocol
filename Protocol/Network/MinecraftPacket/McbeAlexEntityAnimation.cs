using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeAlexEntityAnimation : Packet
{
    public string boneId;
    public AnimationKey[] keys;
    public ulong runtimeEntityId;
    public McbeAlexEntityAnimation()
    {
        Id = 0xe0;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(boneId);
        Write(keys);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        boneId = ReadString();
        keys = ReadAnimationKeys();
    }
}
