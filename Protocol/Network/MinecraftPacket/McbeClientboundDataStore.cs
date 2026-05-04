using Protocol.Minecraft;
using Protocol.Minecraft.Level;

namespace Protocol.Network.MinecraftPacket;

public class McbeClientboundDataStore : Packet
{
    public McbeClientboundDataStore()
    {
        Id = 330;
        IsMcbe = true;
    }

    public DataStoreChangeEntry[] Updates { get; set; } = [];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSlice(Updates, Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Updates = ReadSlice(ReadDataStoreChangeEntry);
    }
}
