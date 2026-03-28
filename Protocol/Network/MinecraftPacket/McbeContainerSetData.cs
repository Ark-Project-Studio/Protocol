namespace Protocol.Network.MinecraftPacket;
public class McbeContainerSetData : Packet
{
    public int property;
    public int value;
    public byte windowId;
    public McbeContainerSetData()
    {
        Id = 0x33;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(windowId);
        WriteSignedVarInt(property);
        WriteSignedVarInt(value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = ReadByte();
        property = ReadSignedVarInt();
        value = ReadSignedVarInt();
    }
}
