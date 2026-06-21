using Protocol.Network;

namespace Protocol.Codec.Packets;
public class EducationSharedResourceURI
{
    public EducationSharedResourceURI()
    {
    }

    public EducationSharedResourceURI(string buttonName, string linkUri)
    {
        ButtonName = buttonName ?? string.Empty;
        LinkURI = linkUri ?? string.Empty;
    }

    public string ButtonName { get; set; } = string.Empty;
    public string LinkURI { get; set; } = string.Empty;
}

public class McbeEducationResourceURI : Packet
{
    public McbeEducationResourceURI()
    {
        Id = 170;
        IsMcbe = true;
    }

    public EducationSharedResourceURI Resource { get; set; } = new();

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Resource?.ButtonName ?? string.Empty);
        Write(Resource?.LinkURI ?? string.Empty);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var buttonName = ReadString();
        var linkUri = ReadString();
        Resource = new EducationSharedResourceURI(buttonName, linkUri);
    }
}
