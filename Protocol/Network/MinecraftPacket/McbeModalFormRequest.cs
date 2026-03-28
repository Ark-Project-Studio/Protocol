namespace Protocol.Network.MinecraftPacket;
public class McbeModalFormRequest : Packet
{
    public string formData;
    public uint formId;
    public McbeModalFormRequest()
    {
        Id = 0x64;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(formId);
        Write(formData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        formId = ReadUnsignedVarInt();
        formData = ReadString();
    }
}
