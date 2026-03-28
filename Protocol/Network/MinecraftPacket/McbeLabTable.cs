namespace Protocol.Network.MinecraftPacket;
public class McbeLabTable : Packet
{
    public int labTableX;
    public int labTableY;
    public int labTableZ;
    public byte reactionType;
    public byte uselessByte;
    public McbeLabTable()
    {
        Id = 0x6d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(uselessByte);
        WriteVarInt(labTableX);
        WriteVarInt(labTableY);
        WriteVarInt(labTableZ);
        Write(reactionType);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        uselessByte = ReadByte();
        labTableX = ReadVarInt();
        labTableY = ReadVarInt();
        labTableZ = ReadVarInt();
        reactionType = ReadByte();
    }
}
