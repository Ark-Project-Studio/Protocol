using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeUpdateBlock : Packet
{
   

    public uint flag;
    public uint blockRuntimeId;
    public BlockCoordinates coordinates;
    public uint layer;
    public McbeUpdateBlock()
    {
        Id = 0x15;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(coordinates);
        WriteUnsignedVarInt(blockRuntimeId);
        WriteUnsignedVarInt(flag);
        WriteUnsignedVarInt(layer);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        blockRuntimeId = ReadUnsignedVarInt();
        flag = ReadUnsignedVarInt();
        layer = ReadUnsignedVarInt();
    }
}
