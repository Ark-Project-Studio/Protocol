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

    public string[] resourcepackids;
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
        WriteSliceUint16Length(resourcepackids ?? [], Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        responseStatus = (ResponseStatus)ReadByte();
        resourcepackids = ReadSliceUint16Length(ReadString);
    }
}
