namespace Protocol.Network.MinecraftPacket;
public class McbeEntityPickRequest : Packet
{
    public bool addUserData;
    public ulong runtimeEntityId;
    public byte selectedSlot;
    public McbeEntityPickRequest()
    {
        Id = 0x23;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(runtimeEntityId);
        Write(selectedSlot);
        Write(addUserData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUlong();
        selectedSlot = ReadByte();
        addUserData = ReadBool();
    }
}
