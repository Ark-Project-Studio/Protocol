namespace Protocol.Network.MinecraftPacket;
public class McbeContainerClose : Packet
{
    public bool server;
    public ContainerType containerType;
    public ContainerID windowId;

    public McbeContainerClose()
    {
        Id = 0x2f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)windowId);
        Write((byte)containerType);
        Write(server);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = (ContainerID)ReadByte();
        containerType = (ContainerType)ReadByte();
        server = ReadBool();
    }
}
