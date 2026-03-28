namespace Protocol.Network.MinecraftPacket;
public class McbeClientCacheBlobStatus : Packet
{
    public ulong[] hashHits;
    public ulong[] hashMisses;
    public McbeClientCacheBlobStatus()
    {
        Id = 0x87;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)hashMisses.Length);
        WriteUnsignedVarInt((uint)hashHits.Length);
        WriteSpecial(hashMisses);
        WriteSpecial(hashHits);
    }

    public void WriteSpecial(ulong[] values)
    {
        if (values == null)
            return;
        if (values.Length == 0)
            return;
        for (var i = 0; i < values.Length; i++)
        {
            var val = values[i];
            Write(val);
        }
    }

    public ulong[] ReadUlongsSpecial(uint len)
    {
        var values = new ulong[len];
        for (var i = 0; i < values.Length; i++)
            values[i] = ReadUlong();
        return values;
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var lenMisses = ReadUnsignedVarInt();
        var lenHits = ReadUnsignedVarInt();
        hashMisses = ReadUlongsSpecial(lenMisses);
        hashHits = ReadUlongsSpecial(lenHits);
    }
}
