using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeChangeMobProperty : Packet
{
    public McbeChangeMobProperty()
    {
        Id = 182;
        IsMcbe = true;
    }

    public ulong EntityUniqueID { get; set; }
    public string Property { get; set; } = string.Empty;
    public bool BoolValue { get; set; }
    public string StringValue { get; set; } = string.Empty;
    public int IntValue { get; set; }
    public float FloatValue { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(EntityUniqueID);
        Write(Property);
        Write(BoolValue);
        Write(StringValue);
        WriteSignedVarInt(IntValue);
        Write(FloatValue);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EntityUniqueID = ReadUlong();
        Property = ReadString();
        BoolValue = ReadBool();
        StringValue = ReadString();
        IntValue = ReadSignedVarInt();
        FloatValue = ReadFloat();
    }
}
