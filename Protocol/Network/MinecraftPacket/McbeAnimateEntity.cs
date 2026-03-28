namespace Protocol.Network.MinecraftPacket;
public class McbeAnimateEntity : Packet
{
    public string animationName;
    public float blendOutTime;
    public string controllerName;
    public ulong[] entities;
    public int molangVersion;
    public string nextState;
    public string stopExpression;
    public McbeAnimateEntity()
    {
        Id = 0x9e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(animationName);
        Write(nextState);
        Write(stopExpression);
        Write(molangVersion);
        Write(controllerName);
        Write(blendOutTime);
        WriteUnsignedVarInt((uint)entities.Length);
        for (var i = 0; i < entities.Length; i++)
            WriteUnsignedVarLong(entities[i]);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        animationName = ReadString();
        nextState = ReadString();
        stopExpression = ReadString();
        molangVersion = ReadInt();
        controllerName = ReadString();
        blendOutTime = ReadFloat();
        var count = ReadUnsignedVarInt();
        entities = new ulong[count];
        for (var i = 0; i < count; i++)
            entities[i] = ReadUnsignedVarLong();
    }
}
