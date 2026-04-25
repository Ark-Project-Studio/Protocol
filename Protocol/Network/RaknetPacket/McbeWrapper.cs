namespace Protocol.Network.RaknetPacket;
public class McbeWrapper : Packet
{
    public ReadOnlyMemory<byte> payload;
    public McbeWrapper()
    {
        Id = 0xfe;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(payload);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        payload = ReadReadOnlyMemory(0, true);
    }
}
