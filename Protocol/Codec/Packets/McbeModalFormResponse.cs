using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeModalFormResponse : Packet
{
    public Optional<byte> cancelReason = new();
    public Optional<string> data = new();
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
        Write(data.HasValue);
        if (data.HasValue)
            Write(data.Value);
        Write(cancelReason.HasValue);
        if (cancelReason.HasValue)
            Write(cancelReason.Value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        formId = ReadUnsignedVarInt();
        if (ReadBool())
            data = new Optional<string>(ReadString());
        if (ReadBool())
            cancelReason = new Optional<byte>(ReadByte());
    }
}
