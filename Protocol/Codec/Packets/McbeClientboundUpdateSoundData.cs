using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeClientboundUpdateSoundData : Packet
{
    public ulong Handle { get; set; }
    public string SoundEvent { get; set; } = string.Empty;

    public McbeClientboundUpdateSoundData()
    {
        Id = 348;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(Handle);
        Write(SoundEvent);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Handle = ReadUnsignedVarLong();
        SoundEvent = ReadString();
    }
}
