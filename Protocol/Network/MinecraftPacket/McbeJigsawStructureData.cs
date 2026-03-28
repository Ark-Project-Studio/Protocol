using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeJigsawStructureData : Packet
{
    public McbeJigsawStructureData()
    {
        Id = 313;
        IsMcbe = true;
        StructureData = null;
    }

    public Nbt StructureData { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(StructureData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        StructureData = ReadNbt();
    }
}
