namespace Protocol.Network.MinecraftPacket;
public class McbeServerSettingsResponse : Packet
{
    public string data;
    public ulong formId;
    public McbeServerSettingsResponse()
    {
        Id = 0x67;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(formId);
        Write(data);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        formId = ReadUnsignedVarLong();
        data = ReadString();
    }
}
