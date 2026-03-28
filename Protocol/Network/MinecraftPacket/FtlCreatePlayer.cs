using Protocol.Minecraft.Skins;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class FtlCreatePlayer : Packet
{
    public long clientId;
    public UUID clientuuid;
    public string serverAddress;
    public Skin skin;
    public string username;
    public FtlCreatePlayer()
    {
        Id = 0x01;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(username);
        Write(clientuuid);
        Write(serverAddress);
        Write(clientId);
        Write(skin);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        username = ReadString();
        clientuuid = ReadUUID();
        serverAddress = ReadString();
        clientId = ReadLong();
        skin = ReadSkin();
    }
}
