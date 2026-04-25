using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeLecternUpdate : Packet
{
    public byte page;
    public byte totalPages;
    public BlockCoordinates position;

    public McbeLecternUpdate()
    {
        Id = 0x7d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(page);
        Write(totalPages);
        Write(position);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        page = ReadByte();
        totalPages = ReadByte();
        position = ReadBlockCoordinates();
    }
}
