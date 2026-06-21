using Protocol.Network;

namespace Protocol.Codec.Packets;
public class GenerationFeature
{
    public string Name { get; set; } = string.Empty;
    public byte[] JSON { get; set; } = new byte[0];
}

public class McbeFeatureRegistry : Packet
{
    public McbeFeatureRegistry()
    {
        Id = 191;
        IsMcbe = true;
    }

    public GenerationFeature[] Features { get; set; } = new GenerationFeature[0];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)(Features?.Length ?? 0));
        if (Features != null)
            foreach (var feature in Features)
            {
                Write(feature.Name ?? string.Empty);
                WriteByteArray(feature.JSON ?? new byte[0]);
            }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var count = ReadUnsignedVarInt();
        Features = new GenerationFeature[count];
        for (var i = 0; i < count; i++)
        {
            var name = ReadString();
            var json = ReadByteArray(true);
            Features[i] = new GenerationFeature
            {
                Name = name,
                JSON = json
            };
        }
    }
}
