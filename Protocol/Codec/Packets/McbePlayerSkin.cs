using Protocol.Minecraft.Actor.Player;
using Protocol.Network;
using Protocol.Utils;

namespace Protocol.Codec.Packets;
public class McbePlayerSkin : Packet
{
    public bool isVerified;
    public string oldSkinName;
    public Skin skin;
    public string skinName;
    public UUID uuid;
    public McbePlayerSkin()
    {
        Id = 0x5d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(uuid);
        Write(skin);
        Write(skinName);
        Write(oldSkinName);
        Write(isVerified);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        uuid = ReadUUID();
        skin = ReadSkin();
        skinName = ReadString();
        oldSkinName = ReadString();
        isVerified = ReadBool();
    }
}
