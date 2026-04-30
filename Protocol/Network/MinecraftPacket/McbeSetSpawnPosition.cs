using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeSetSpawnPosition : Packet
{
    public BlockCoordinates coordinates;
    public int dimension;
    public int spawnType;
    public BlockCoordinates spawnBlockPosition;
    public McbeSetSpawnPosition()
    {
        Id = 0x2b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(spawnType);
        Write(coordinates);
        WriteSignedVarInt(dimension);
        Write(spawnBlockPosition);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        spawnType = ReadSignedVarInt();
        coordinates = ReadBlockCoordinates();
        dimension = ReadSignedVarInt();
        spawnBlockPosition = ReadBlockCoordinates();
    }
}
