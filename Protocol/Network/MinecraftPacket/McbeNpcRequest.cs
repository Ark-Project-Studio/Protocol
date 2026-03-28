namespace Protocol.Network.MinecraftPacket;
public class McbeNpcRequest : Packet
{
    public ulong runtimeEntityId;
    public byte unknown0;
    public string unknown1;
    public byte unknown2;
    public McbeNpcRequest()
    {
        Id = 0x62;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(unknown0);
        Write(unknown1);
        Write(unknown2);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        unknown0 = ReadByte();
        unknown1 = ReadString();
        unknown2 = ReadByte();
    }
}
