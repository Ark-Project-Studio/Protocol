using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeRemoveVolumeEntity : Packet
{
    public McbeRemoveVolumeEntity()
    {
        Id = 167;
        IsMcbe = true;
    }

    public ulong EntityRuntimeID { get; set; }
    public int Dimension { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(EntityRuntimeID);
        WriteSignedVarInt(Dimension);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EntityRuntimeID = ReadUlong();
        Dimension = ReadSignedVarInt();
    }
}
