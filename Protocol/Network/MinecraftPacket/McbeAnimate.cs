namespace Protocol.Network.MinecraftPacket;
public class McbeAnimate : Packet
{
    public int actionId;
    public ulong runtimeEntityId;
    public float Data;
    public float unknownFloat;
    public McbeAnimate()
    {
        Id = 0x2c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(actionId);
        WriteUnsignedVarLong(runtimeEntityId);
        Write(Data);
        if (actionId == 0x80 || actionId == 0x81)
            Write(unknownFloat);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        actionId = ReadSignedVarInt();
        runtimeEntityId = ReadUnsignedVarLong();
        Data = ReadFloat();
        if (actionId == 0x80 || actionId == 0x81)
            unknownFloat = ReadFloat();
    }
}
