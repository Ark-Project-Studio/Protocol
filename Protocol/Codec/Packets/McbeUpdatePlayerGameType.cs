using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeUpdatePlayerGameType : Packet
{
	public int GameType { get; set; }
	public long PlayerUniqueID { get; set; }
	public ulong Tick { get; set; }
	
	public McbeUpdatePlayerGameType()
    {
        Id = 0x97;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		WriteVarInt(GameType);
		WriteVarLong(PlayerUniqueID);
		WriteUnsignedVarLong(Tick);
	}

	protected override void DecodePacket()
    {
        base.DecodePacket();
		GameType = ReadVarInt();
		PlayerUniqueID = ReadVarLong();
		Tick = (ulong)ReadUnsignedVarLong();
	}
}
