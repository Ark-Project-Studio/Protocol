using Protocol.Network;

namespace Protocol.Codec.Packets;
public static class AgentActionType
{
    public const int Attack = 1;
    public const int Collect = 2;
    public const int Destroy = 3;
    public const int DetectRedstone = 4;
    public const int DetectObstacle = 5;
    public const int Drop = 6;
    public const int DropAll = 7;
    public const int Inspect = 8;
    public const int InspectData = 9;
    public const int InspectItemCount = 10;
    public const int InspectItemDetail = 11;
    public const int InspectItemSpace = 12;
    public const int Interact = 13;
    public const int Move = 14;
    public const int PlaceBlock = 15;
    public const int Till = 16;
    public const int TransferItemTo = 17;
    public const int Turn = 18;
}

public class McbeAgentAction : Packet
{
    public McbeAgentAction()
    {
        Id = 181;
        IsMcbe = true;
    }

    public string Identifier { get; set; } = string.Empty;
    public int Action { get; set; }
    public byte[] Response { get; set; } = new byte[0];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Identifier);
        WriteSignedVarInt(Action);
        WriteByteArray(Response);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Identifier = ReadString();
        Action = ReadSignedVarInt();
        Response = ReadByteArray(true);
    }
}
