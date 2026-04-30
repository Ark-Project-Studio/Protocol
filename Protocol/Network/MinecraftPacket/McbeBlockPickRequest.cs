namespace Protocol.Network.MinecraftPacket;
public class McbeBlockPickRequest : Packet
{
    public bool withData;
    public byte maxSlots;
    public int x;
    public int y;
    public int z;
    public McbeBlockPickRequest()
    {
        Id = 0x22;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(x);
        WriteSignedVarInt(y);
        WriteSignedVarInt(z);
        Write(withData);
        Write(maxSlots);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        x = ReadSignedVarInt();
        y = ReadSignedVarInt();
        z = ReadSignedVarInt();
        withData = ReadBool();
        maxSlots = ReadByte();
    }
}
