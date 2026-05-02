namespace Protocol.Network.MinecraftPacket;
public enum ObjectiveSortOrder : int
{
    Ascending = 0,
    Descending = 1
}

public class McbeSetDisplayObjective : Packet
{
    public string criteriaName;
    public string displayName;
    public string displaySlot;
    public string objectiveName;
    public ObjectiveSortOrder sortOrder;
    public McbeSetDisplayObjective()
    {
        Id = 0x6b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(displaySlot);
        Write(objectiveName);
        Write(displayName);
        Write(criteriaName);
        WriteSignedVarInt((int)sortOrder);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        displaySlot = ReadString();
        objectiveName = ReadString();
        displayName = ReadString();
        criteriaName = ReadString();
        sortOrder = (ObjectiveSortOrder)ReadSignedVarInt();
    }
}
