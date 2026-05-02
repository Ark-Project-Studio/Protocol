namespace Protocol.Network.MinecraftPacket;
public class McbeSetEntityLink : Packet
{
    public enum ActorLinkType : byte
    {
        None = 0,
        Riding = 1,
        Passenger = 2
    }

    public ActorLinkType linkType;
    public bool passengerInitiated;
    public long riddenId;
    public long riderId;
    public bool immediate;
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
        Write((byte)linkType);
        Write(immediate);
        Write(passengerInitiated);
        Write(vehicleAngularVelocity);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        riddenId = ReadSignedVarLong();
        riderId = ReadSignedVarLong();
        linkType = (ActorLinkType)ReadByte();
        immediate = ReadBool();
        passengerInitiated = ReadBool();
        vehicleAngularVelocity = ReadFloat();
    }
}
