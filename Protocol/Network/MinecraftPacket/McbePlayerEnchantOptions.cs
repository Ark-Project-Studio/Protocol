using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbePlayerEnchantOptions : Packet
{
    public EnchantOptions enchantOptions;
    public McbePlayerEnchantOptions()
    {
        Id = 0x92;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(enchantOptions);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        enchantOptions = ReadEnchantOptions();
    }
}
