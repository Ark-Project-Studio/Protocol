using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeAddBehaviorTree : Packet
{
    public string behaviortree;
    public McbeAddBehaviorTree()
    {
        Id = 0x59;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(behaviortree);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        behaviortree = ReadString();
    }
}
