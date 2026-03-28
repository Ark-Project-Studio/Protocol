namespace Protocol.Network.MinecraftPacket;
public class McbeBlockPickRequest : Packet
{
    public bool addUserData;
    public byte selectedSlot;
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
        Write(addUserData);
        Write(selectedSlot);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        x = ReadSignedVarInt();
        y = ReadSignedVarInt();
        z = ReadSignedVarInt();
        addUserData = ReadBool();
        selectedSlot = ReadByte();
    }
}
