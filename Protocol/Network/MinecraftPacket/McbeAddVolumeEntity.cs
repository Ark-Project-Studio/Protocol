using Protocol.Minecraft;
using Protocol.Minecraft.Level.Block;
using Protocol.Minecraft.NBT;

namespace Protocol.Network.MinecraftPacket;
public class McbeAddVolumeEntity : Packet
{
    public McbeAddVolumeEntity()
    {
        Id = 166;
        IsMcbe = true;
    }

    public uint EntityRuntimeID { get; set; }
    public Nbt EntityMetadata { get; set; } = new Nbt();
    public string EncodingIdentifier { get; set; } = string.Empty;
    public string InstanceIdentifier { get; set; } = string.Empty;
    public BlockCoordinates[] Bounds { get; set; } = new BlockCoordinates[2];
    public int Dimension { get; set; }
    public string EngineVersion { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(EntityRuntimeID);
        Write(EntityMetadata);
        Write(EncodingIdentifier);
        Write(InstanceIdentifier);
        Write(Bounds[0]);
        Write(Bounds[1]);
        WriteSignedVarInt(Dimension);
        Write(EngineVersion);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EntityRuntimeID = ReadUnsignedVarInt();
        EntityMetadata = ReadNbt();
        EncodingIdentifier = ReadString();
        InstanceIdentifier = ReadString();
        Bounds = new BlockCoordinates[2];
        Bounds[0] = ReadBlockCoordinates();
        Bounds[1] = ReadBlockCoordinates();
        Dimension = ReadSignedVarInt();
        EngineVersion = ReadString();
    }
}
