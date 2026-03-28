using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeCreativeContent : Packet
{
    public List<creativeGroup> groups;
    public List<CreativeItemEntry> input;
    public McbeCreativeContent()
    {
        Id = 0x91;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(groups);
        Write(input);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        groups = ReadCreativeGroups();
        input = ReadCreativeItemStacks();
    }
}
