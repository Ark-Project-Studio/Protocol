using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbePlayerHotbar : Packet
{
    public uint selectedSlot;
    public bool selectSlot;
    public byte windowId;
    public McbePlayerHotbar()
    {
        Id = 0x30;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(selectedSlot);
        Write(windowId);
        Write(selectSlot);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        selectedSlot = ReadUnsignedVarInt();
        windowId = ReadByte();
        selectSlot = ReadBool();
    }
}
