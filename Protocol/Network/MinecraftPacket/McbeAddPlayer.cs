using Protocol.Minecraft;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class McbeAddPlayer : Packet
{
    public byte commandPermissions;
    public string deviceId;
    public int deviceOs;
    public long entityIdSelf;
    public uint gameType;
    public float headYaw;
    public Item item;
    public AbilityLayers layers;
    public EntityLinks links;
    public MetadataDictionary metadata;
    public float pitch;
    public string platformChatId;
    public byte playerPermissions;
    public ulong runtimeEntityId;
    public float speedX;
    public float speedY;
    public float speedZ;
    public PropertySyncData syncdata;
    public string username;
    public UUID uuid;
    public float x;
    public float y;
    public float yaw;
    public float z;
    public McbeAddPlayer()
    {
        Id = 0x0c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(uuid);
        Write(username);
        WriteUnsignedVarLong(runtimeEntityId);
        Write(platformChatId);
        Write(x);
        Write(y);
        Write(z);
        Write(speedX);
        Write(speedY);
        Write(speedZ);
        Write(pitch);
        Write(yaw);
        Write(headYaw);
        Write(item);
        WriteUnsignedVarInt(gameType);
        Write(metadata);
        Write(syncdata);
        Write((ulong)entityIdSelf);
        Write(playerPermissions);
        Write(commandPermissions);
        Write(layers);
        Write(links);
        Write(deviceId);
        Write(deviceOs);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        uuid = ReadUUID();
        username = ReadString();
        runtimeEntityId = ReadUnsignedVarLong();
        platformChatId = ReadString();
        x = ReadFloat();
        y = ReadFloat();
        z = ReadFloat();
        speedX = ReadFloat();
        speedY = ReadFloat();
        speedZ = ReadFloat();
        pitch = ReadFloat();
        yaw = ReadFloat();
        headYaw = ReadFloat();
        item = ReadItem();
        gameType = ReadUnsignedVarInt();
        metadata = ReadMetadataDictionary();
        syncdata = ReadPropertySyncData();
        entityIdSelf = ReadSignedVarLong();
        playerPermissions = ReadByte();
        commandPermissions = ReadByte();
        layers = ReadAbilityLayers();
        links = ReadEntityLinks();
        deviceId = ReadString();
        deviceOs = ReadInt();
    }
}
