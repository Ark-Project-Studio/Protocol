using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbePlayerLocation : Packet
{
    public enum Type : int
    {
        PlayerLocationCoordinates = 0,
        PlayerLocationHide = 1
    }

    public McbePlayerLocation()
    {
        Id = 326;
        IsMcbe = true;
    }

    public Type PlayerLocationType { get; set; }
    public long EntityUniqueID { get; set; }
    public Vector3 Position { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((int)PlayerLocationType);
        WriteSignedVarLong(EntityUniqueID);
        if (PlayerLocationType == Type.PlayerLocationCoordinates)
            Write(Position);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        PlayerLocationType = (Type)ReadInt();
        EntityUniqueID = ReadSignedVarLong();
        if (PlayerLocationType == Type.PlayerLocationCoordinates)
            Position = ReadVector3();
        else
            Position = Vector3.Zero;
    }
}
