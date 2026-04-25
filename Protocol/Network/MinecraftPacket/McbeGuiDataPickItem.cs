namespace Protocol.Network.MinecraftPacket;
public class McbeGuiDataPickItem : Packet
{
    public string itemName;
    public string itemEffectName;
    public int slot;

    public McbeGuiDataPickItem()
    {
        Id = 0x36;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(itemName);
        Write(itemEffectName);
        Write(slot);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        itemName = ReadString();
        itemEffectName = ReadString();
        slot = ReadInt();
    }
}
