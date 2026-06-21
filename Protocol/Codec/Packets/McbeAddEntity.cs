using Protocol.Minecraft;
using Protocol.Minecraft.Actor;
using Protocol.Minecraft.Actor.Player;
using Protocol.Minecraft.Metadata;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeAddEntity : Packet
{
    public EntityAttributes attributes;
    public float bodyYaw;
    public long entityIdSelf;
    public string entityType;
    public float headYaw;
    public EntityLink[] links;
    public MetadataDictionary metadata;
    public float pitch;
    public ulong runtimeEntityId;
    public float speedX;
    public float speedY;
    public float speedZ;
    public PropertySyncData syncdata;
    public float x;
    public float y;
    public float yaw;
    public float z;
    public McbeAddEntity()
    {
        Id = 0x0d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(entityIdSelf);
        WriteUnsignedVarLong(runtimeEntityId);
        Write(entityType);
        Write(x);
        Write(y);
        Write(z);
        Write(speedX);
        Write(speedY);
        Write(speedZ);
        Write(pitch);
        Write(yaw);
        Write(headYaw);
        Write(bodyYaw);
        Write(attributes);
        Write(metadata);
        Write(syncdata);
        Write(links);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entityIdSelf = ReadSignedVarLong();
        runtimeEntityId = ReadUnsignedVarLong();
        entityType = ReadString();
        x = ReadFloat();
        y = ReadFloat();
        z = ReadFloat();
        speedX = ReadFloat();
        speedY = ReadFloat();
        speedZ = ReadFloat();
        pitch = ReadFloat();
        yaw = ReadFloat();
        headYaw = ReadFloat();
        bodyYaw = ReadFloat();
        attributes = ReadEntityAttributes();
        metadata = ReadMetadataDictionary();
        syncdata = ReadPropertySyncData();
        links = ReadEntityLinks();
    }
}
