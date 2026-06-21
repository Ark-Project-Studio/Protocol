using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeCurrentStructureFeature : Packet
{
    public McbeCurrentStructureFeature()
    {
        Id = 314;
        IsMcbe = true;
    }

    public string CurrentFeature { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(CurrentFeature);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        CurrentFeature = ReadString();
    }
}
