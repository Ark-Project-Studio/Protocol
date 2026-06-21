using Protocol.Minecraft;
using Protocol.Minecraft.NBT;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeLevelEventGeneric : Packet
{
	public int EventID { get; set; }
	public CompoundTag SerialisedEventData { get; set; }
	public McbeLevelEventGeneric()
    {
        Id = 0x7c;
        IsMcbe = true;
        SerialisedEventData = new CompoundTag();
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(EventID);
        Write(SerialisedEventData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EventID = ReadSignedVarInt();
		SerialisedEventData = ReadCompoundTag();
    }
}
