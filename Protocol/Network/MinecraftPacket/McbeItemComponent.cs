using Protocol.Minecraft.Actor.Player;

namespace Protocol.Network.MinecraftPacket;
public class McbeItemComponent : Packet
{
    public Itemstate[] entries;
    public McbeItemComponent()
    {
        Id = 0xa2;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(entries);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entries = ReadItemstates();
    }
}
