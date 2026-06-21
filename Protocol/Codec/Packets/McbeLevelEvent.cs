using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbeLevelEvent : Packet
{
    public int data;
    public int eventId;
    public Vector3 position;
    public McbeLevelEvent()
    {
        Id = 0x19;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(eventId);
        Write(position);
        WriteSignedVarInt(data);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        eventId = ReadSignedVarInt();
        position = ReadVector3();
        data = ReadSignedVarInt();
    }
}
