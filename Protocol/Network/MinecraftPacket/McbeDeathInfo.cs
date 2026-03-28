namespace Protocol.Network.MinecraftPacket;
public class McbeDeathInfo : Packet
{
    public McbeDeathInfo()
    {
        Id = 189;
        IsMcbe = true;
    }

    public string Cause { get; set; } = string.Empty;
    public string[] Messages { get; set; } = new string[0];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Cause);
        WriteUnsignedVarInt((uint)(Messages?.Length ?? 0));
        if (Messages != null)
            foreach (var message in Messages)
                Write(message ?? string.Empty);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Cause = ReadString();
        var count = ReadUnsignedVarInt();
        Messages = new string[count];
        for (var i = 0; i < count; i++)
            Messages[i] = ReadString();
    }
}
