using Protocol.Minecraft;
using Protocol.Minecraft.Level.Chunk;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeSubChunkRequestPacket : Packet
{
	public int Dimension { get; set; }
	public SubChunkPos Position { get; set; }
	public System.Collections.Generic.List<SubChunkOffset> Offsets { get; set; }
	public McbeSubChunkRequestPacket()
    {
        Id = 0xaf;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		WriteSignedVarInt(Dimension);

		if (Offsets != null)
		{
			WriteSliceUint32Length(Offsets.ToArray(), Write);
		}
		else
		{
			Write((uint)0);
		}
		WriteSubChunkPosCereal(Position);
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();

        Dimension = ReadSignedVarInt();

		uint count = ReadUint();
		var offsetsArray = new SubChunkOffset[count];
		for (int i = 0; i < count; i++)
		{
			offsetsArray[i] = ReadSubChunkOffset();
		}
		Offsets = new System.Collections.Generic.List<SubChunkOffset>(offsetsArray);
		Position = ReadSubChunkPosCereal();
	}
}
