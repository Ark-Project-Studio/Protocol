using Protocol.Minecraft;
using Protocol.Minecraft.Inventory.Enchant;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbePlayerEnchantOptions : Packet
{
    public EnchantOption[] enchantOptions;
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
