using Protocol.Minecraft.Level.Block;

namespace Protocol.Network.MinecraftPacket;
public class McbeAddPainting : Packet
{
    public BlockCoordinates coordinates;
    public int direction;
    public long entityIdSelf;
    public ulong runtimeEntityId;
    public string title;
    public McbeAddPainting()
    {
        Id = 0x16;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(entityIdSelf);
        WriteUnsignedVarLong(runtimeEntityId);
        WritePaintingCoordinates(coordinates);
        WriteSignedVarInt(direction);
        Write(title);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entityIdSelf = ReadSignedVarLong();
        runtimeEntityId = ReadUnsignedVarLong();
        coordinates = ReadBlockCoordinates();
        direction = ReadSignedVarInt();
        title = ReadString();
    }
}
