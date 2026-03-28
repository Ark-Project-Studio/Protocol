namespace Protocol.Network.MinecraftPacket;
public class McbeModalFormResponse : Packet
{
    public byte cancelReason;
    public string data = "";
    public uint formId;
    public McbeModalFormResponse()
    {
        Id = 0x65;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(formId);
        Write(data);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        formId = ReadUnsignedVarInt();
        if (ReadBool())
            data = ReadString();
        if (ReadBool())
            cancelReason = ReadByte();
    }
}
