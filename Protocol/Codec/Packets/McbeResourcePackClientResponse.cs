using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeResourcePackClientResponse : Packet
{
    public enum ResponseStatus
    {
        Refused = 1,
        SendPacks = 2,
        HaveAllPacks = 3,
        Completed = 4
    }

    public string[] resourcepackids;
    public byte responseStatus;
    public McbeResourcePackClientResponse()
    {
        Id = 0x08;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(responseStatus);
        Write((ushort)(resourcepackids?.Length ?? 0));
        if (resourcepackids == null) return;

        foreach (var id in resourcepackids)
        {
            Write(id);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        responseStatus = ReadByte();
        var count = ReadUshort();
        resourcepackids = new string[count];
        for (int i = 0; i < count; i++)
        {
            resourcepackids[i] = (ReadString());
        }
    }
}
