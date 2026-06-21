using System.Numerics;
using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeStructureBlockUpdate : Packet
{
    public BlockCoordinates BlockPosition { get; set; }
    public StructureEditorData StructureData { get; set; } = new();
    public bool Trigger { get; set; }
    public bool IsWaterLogged { get; set; }

    public McbeStructureBlockUpdate()
    {
        Id = 0x5a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(BlockPosition);
        Write(StructureData);
        Write(Trigger);
        Write(IsWaterLogged);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        BlockPosition = ReadBlockCoordinates();
        StructureData = ReadStructureEditorData();
        Trigger = ReadBool();
        IsWaterLogged = ReadBool();
    }

   

  
}
