namespace Protocol.Network.MinecraftPacket;
public class McbeClientCacheMissResponse : Packet
{
    public Dictionary<ulong, byte[]> blobs;
    public McbeClientCacheMissResponse()
    {
        Id = 0x88;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        if (blobs == null)
        {
            WriteUnsignedVarInt(0);
            return;
        }

        WriteUnsignedVarInt((uint)blobs.Count);
        foreach (var blob in blobs)
        {
            Write(blob.Key);
            WriteByteArray(blob.Value);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        blobs = new Dictionary<ulong, byte[]>();
        var count = ReadUnsignedVarInt();
        for (var i = 0; i < count; i++)
        {
            var hash = ReadUlong();
            var blob = ReadByteArray();
            if (blobs.ContainsKey(hash))
            {
            }
            else
            {
                blobs.Add(hash, blob);
            }
        }
    }
}
