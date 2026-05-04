using Protocol.Minecraft.Level.ResourcePacks;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class McbeResourcePacksInfo : Packet
{
    public bool ForceDisableVibrantVisuals;
    public bool hasAddons;
    public bool hasScripts;
    public bool mustAccept;
    public UUID templateUUID;
    public string templateVersion;
    public TexturePackInfo[] texturepacks;
    public McbeResourcePacksInfo()
    {
        Id = 0x06;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(mustAccept);
        Write(hasAddons);
        Write(hasScripts);
        Write(ForceDisableVibrantVisuals);
        Write(templateUUID);
        Write(templateVersion);
        WriteSliceUint16Length(texturepacks, Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        mustAccept = ReadBool();
        hasAddons = ReadBool();
        hasScripts = ReadBool();
        ForceDisableVibrantVisuals = ReadBool();
        templateUUID = ReadUUID();
        templateVersion = ReadString();
        texturepacks = ReadSliceUint16Length(ReadTexturePackInfo);
    }
}
