using Protocol.Network;

namespace Protocol.Codec.Packets;
public static class NPCDialogueActionType
{
    public const int Open = 0;
    public const int Close = 1;
}

public class McbeNPCDialogue : Packet
{
    public McbeNPCDialogue()
    {
        Id = 169;
        IsMcbe = true;
    }

    public ulong EntityUniqueID { get; set; }
    public int ActionType { get; set; }
    public string Dialogue { get; set; } = string.Empty;
    public string SceneName { get; set; } = string.Empty;
    public string NPCName { get; set; } = string.Empty;
    public string ActionJSON { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(EntityUniqueID);
        WriteSignedVarInt(ActionType);
        Write(Dialogue);
        Write(SceneName);
        Write(NPCName);
        Write(ActionJSON);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EntityUniqueID = ReadUlong();
        ActionType = ReadSignedVarInt();
        Dialogue = ReadString();
        SceneName = ReadString();
        NPCName = ReadString();
        ActionJSON = ReadString();
    }
}
