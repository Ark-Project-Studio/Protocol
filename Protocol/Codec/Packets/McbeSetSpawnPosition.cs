using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum SpawnPositionType : int
{
    PlayerRespawn = 0,
    WorldSpawn = 1
}

public class McbeSetSpawnPosition : Packet
{
    public BlockCoordinates coordinates;
    public int dimension;
    public SpawnPositionType spawnType;
    public BlockCoordinates spawnBlockPosition;
    public McbeSetSpawnPosition()
    {
        Id = 0x2b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt((int)spawnType);
        Write(coordinates);
        WriteSignedVarInt(dimension);
        Write(spawnBlockPosition);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        spawnType = (SpawnPositionType)ReadSignedVarInt();
        coordinates = ReadBlockCoordinates();
        dimension = ReadSignedVarInt();
        spawnBlockPosition = ReadBlockCoordinates();
    }
}
