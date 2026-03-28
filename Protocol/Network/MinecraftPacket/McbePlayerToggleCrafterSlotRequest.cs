namespace Protocol.Network.MinecraftPacket;
public class McbePlayerToggleCrafterSlotRequest : Packet
{
    public McbePlayerToggleCrafterSlotRequest()
    {
        Id = 306;
        IsMcbe = true;
    }

    public int PosX { get; set; }
    public int PosY { get; set; }
    public int PosZ { get; set; }
    public byte Slot { get; set; }
    public bool Disabled { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(PosX);
        Write(PosY);
        Write(PosZ);
        Write(Slot);
        Write(Disabled);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        PosX = ReadInt();
        PosY = ReadInt();
        PosZ = ReadInt();
        Slot = ReadByte();
        Disabled = ReadBool();
    }
}
