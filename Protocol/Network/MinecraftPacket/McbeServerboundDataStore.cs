using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;

public class McbeServerboundDataStore : Packet
{
    public McbeServerboundDataStore()
    {
        Id = 332;
        IsMcbe = true;
    }

    public DataStoreUpdate Update { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Update);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Update = ReadDataStoreUpdate();
    }
}
