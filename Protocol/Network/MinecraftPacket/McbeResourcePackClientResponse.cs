using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeResourcePackClientResponse : Packet
{
    public enum ResponseStatus
    {
        Refused = 1,
        SendPacks = 2,
        HaveAllPacks = 3,
        Completed = 4
    }

    public ResourcePackIds resourcepackids;
    public ResponseStatus responseStatus;
    public McbeResourcePackClientResponse()
    {
        Id = 0x08;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)responseStatus);
        Write((ushort)(resourcepackids?.Count ?? 0));
        if (resourcepackids == null) return;

        foreach (var id in resourcepackids)
        {
            Write(id);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        responseStatus = (ResponseStatus)ReadByte();
        var count = ReadUshort();
        resourcepackids = new ResourcePackIds();
        for (int i = 0; i < count; i++)
        {
            resourcepackids.Add(ReadString());
        }
    }
}
