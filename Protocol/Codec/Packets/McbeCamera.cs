using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeCamera : Packet
{
    public long cameraId;
    public long targetPlayerId;
    public McbeCamera()
    {
        Id = 0x49;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(cameraId);
        WriteSignedVarLong(targetPlayerId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        cameraId = ReadSignedVarLong();
        targetPlayerId = ReadSignedVarLong();
    }
}
