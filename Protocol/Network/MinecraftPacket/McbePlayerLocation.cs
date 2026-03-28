using System.Numerics;

namespace Protocol.Network.MinecraftPacket;
public class McbePlayerLocation : Packet
{
    public enum PlayerLocation
    {
        PlayerLocationTypeCoordinates = 0,
        PlayerLocationTypeHide = 1
    }

    public McbePlayerLocation()
    {
        Id = 326;
        IsMcbe = true;
    }

    public int Type { get; set; }
    public long EntityUniqueID { get; set; }
    public Vector3 Position { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Type);
        WriteSignedVarLong(EntityUniqueID);
        if (Type == (int)PlayerLocation.PlayerLocationTypeCoordinates)
            Write(Position);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Type = ReadInt();
        EntityUniqueID = ReadSignedVarLong();
        if (Type == (int)PlayerLocation.PlayerLocationTypeCoordinates)
            Position = ReadVector3();
        else
            Position = Vector3.Zero;
    }
}
