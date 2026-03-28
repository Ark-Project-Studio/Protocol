namespace Protocol.Network.MinecraftPacket;
public class McbeSetEntityLink : Packet
{
    public enum LinkActions
    {
        Remove = 0,
        Ride = 1,
        Passenger = 2
    }

    public byte linkType;
    public long riddenId;
    public long riderId;
    public byte unknown;
    public float vehicleAngularVelocity;
    public McbeSetEntityLink()
    {
        Id = 0x29;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(riddenId);
        WriteSignedVarLong(riderId);
        Write(linkType);
        Write(unknown);
        Write(false);
        Write(vehicleAngularVelocity);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        riddenId = ReadSignedVarLong();
        riderId = ReadSignedVarLong();
        linkType = ReadByte();
        unknown = ReadByte();
        vehicleAngularVelocity = ReadFloat();
    }
}
