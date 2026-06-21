using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeVideoStreamConnect : Packet
{
    public byte action;
    public float frameSendFrequency;
    public int resolutionX;
    public int resolutionY;
    public string serverUri;
    public McbeVideoStreamConnect()
    {
        Id = 0x7e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(serverUri);
        Write(frameSendFrequency);
        Write(action);
        Write(resolutionX);
        Write(resolutionY);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        serverUri = ReadString();
        frameSendFrequency = ReadFloat();
        action = ReadByte();
        resolutionX = ReadInt();
        resolutionY = ReadInt();
    }
}
