using Protocol.Minecraft;
using Protocol.Minecraft.Level.Block;
using Protocol.Minecraft.Level.Chunk;

namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateSubChunkBlocksPacket : Packet
{
    public UpdateSubChunkBlocksPacketEntry[] layerOneUpdates;
    public UpdateSubChunkBlocksPacketEntry[] layerZeroUpdates;
    public BlockCoordinates subchunkCoordinates;
    public McbeUpdateSubChunkBlocksPacket()
    {
        Id = 0xac;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(subchunkCoordinates);
        Write(layerZeroUpdates);
        Write(layerOneUpdates);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        subchunkCoordinates = ReadBlockCoordinates();
        layerZeroUpdates = ReadUpdateSubChunkBlocksPacketEntrys();
        layerOneUpdates = ReadUpdateSubChunkBlocksPacketEntrys();
    }
}
