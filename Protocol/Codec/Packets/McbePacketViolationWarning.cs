using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbePacketViolationWarning : Packet
{
    public int packetId;
    public string reason;
    public int severity;
    public int violationType;
    public McbePacketViolationWarning()
    {
        Id = 0x9c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(violationType);
        WriteSignedVarInt(severity);
        WriteSignedVarInt(packetId);
        Write(reason);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        violationType = ReadSignedVarInt();
        severity = ReadSignedVarInt();
        packetId = ReadSignedVarInt();
        reason = ReadString();
    }
}
