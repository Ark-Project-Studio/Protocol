using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeAddItemEntity : Packet
{
    public long entityIdSelf;
    public bool isFromFishing;
    public NetworkItemStackDescriptor item;
    public MetadataDictionary metadata;
    public ulong runtimeEntityId;
    public float speedX;
    public float speedY;
    public float speedZ;
    public float x;
    public float y;
    public float z;
    public McbeAddItemEntity()
    {
        Id = 0x0f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(entityIdSelf);
        WriteUnsignedVarLong(runtimeEntityId);
        Write(item);
        Write(x);
        Write(y);
        Write(z);
        Write(speedX);
        Write(speedY);
        Write(speedZ);
        Write(metadata);
        Write(isFromFishing);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entityIdSelf = ReadSignedVarLong();
        runtimeEntityId = ReadUnsignedVarLong();
        item = ReadNetworkItemStackDescriptor();
        x = ReadFloat();
        y = ReadFloat();
        z = ReadFloat();
        speedX = ReadFloat();
        speedY = ReadFloat();
        speedZ = ReadFloat();
        metadata = ReadMetadataDictionary();
        isFromFishing = ReadBool();
    }
}
