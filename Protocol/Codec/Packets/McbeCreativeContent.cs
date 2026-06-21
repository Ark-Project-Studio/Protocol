using Protocol.Minecraft.Inventory.Item;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeCreativeContent : Packet
{
    public List<CreativeContentGroup> groups = new();
    public List<CreativeContentWriteEntry> writeEntries = new();

    public McbeCreativeContent()
    {
        Id = 0x91;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(groups);
        Write(writeEntries);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        groups = ReadCreativeContentGroups();
        writeEntries = ReadCreativeContentWriteEntries();
    }
}
