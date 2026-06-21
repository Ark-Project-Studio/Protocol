using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeScriptMessage : Packet
{
    public McbeScriptMessage()
    {
        Id = 177;
        IsMcbe = true;
    }

    public string Identifier { get; set; } = string.Empty;
    public byte[] Data { get; set; } = new byte[0];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Identifier);
        WriteByteArray(Data);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Identifier = ReadString();
        Data = ReadByteArray(true);
    }
}
