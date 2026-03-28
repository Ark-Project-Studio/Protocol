namespace Protocol.Network.MinecraftPacket;
public partial class McbeFilterTextPacket : Packet
{
    public bool fromServer;
    public string text;
    public McbeFilterTextPacket()
    {
        Id = 0xa3;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        BeforeEncode();
        Write(text);
        Write(fromServer);
        AfterEncode();
    }

    partial void BeforeEncode();
    partial void AfterEncode();
    protected override void DecodePacket()
    {
        base.DecodePacket();
        BeforeDecode();
        text = ReadString();
        fromServer = ReadBool();
        AfterDecode();
    }

    partial void BeforeDecode();
    partial void AfterDecode();
}
