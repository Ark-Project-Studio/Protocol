using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeSyncEntityProperty : Packet
{
    public Nbt propertyData;
    public McbeSyncEntityProperty()
    {
        Id = 0xa5;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(propertyData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        propertyData = ReadNbt();
    }
}
