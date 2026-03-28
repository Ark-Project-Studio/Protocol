namespace Protocol.Network.MinecraftPacket;
public class McbeSetInventoryOptions : Packet
{
    public int craftingLayout;
    public bool filtering;
    public int inventoryLayout;
    public int leftTab;
    public int rightTab;
    public McbeSetInventoryOptions()
    {
        Id = 0x133;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        leftTab = ReadSignedVarInt();
        rightTab = ReadSignedVarInt();
        filtering = ReadBool();
        inventoryLayout = ReadSignedVarInt();
        craftingLayout = ReadSignedVarInt();
    }
}
