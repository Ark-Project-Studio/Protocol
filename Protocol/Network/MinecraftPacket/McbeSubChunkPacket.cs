using Protocol.Minecraft;
using Protocol.Minecraft.Level.Chunk;

namespace Protocol.Network.MinecraftPacket;
public class McbeSubChunkPacket : Packet
{
	public bool CacheEnabled { get; set; }
	public int Dimension { get; set; }
	public SubChunkPos Position { get; set; }
	public System.Collections.Generic.List<SubChunkEntry> SubChunkEntries { get; set; }
	public McbeSubChunkPacket()
    {
        Id = 0xae;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		Write(CacheEnabled);
		WriteSignedVarInt(Dimension);
		Write(Position);

		if (CacheEnabled)
		{
			if (SubChunkEntries != null)
			{
				WriteSliceUint32Length(SubChunkEntries.ToArray(), Write);
			}
			else
			{
				Write((uint)0);
			}
		}
		else
		{
			if (SubChunkEntries != null)
			{
				WriteSliceUint32Length(SubChunkEntries.ToArray(), WriteSubChunkEntryNoCache);
			}
			else
			{
				Write((uint)0);
			}
		}
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();

        CacheEnabled = ReadBool();
        Dimension = ReadSignedVarInt();
        Position = ReadSubChunkPos();
		

		uint count = ReadUint();
		SubChunkEntries = new System.Collections.Generic.List<SubChunkEntry>((int)count);

		if (CacheEnabled)
		{
			for (int i = 0; i < count; i++)
			{
				SubChunkEntries.Add(ReadSubChunkEntry());
			}
		}
		else
		{
			for (int i = 0; i < count; i++)
			{
				SubChunkEntries.Add(ReadSubChunkEntryNoCache());
			}
		}
	}
}