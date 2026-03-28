using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeCommandBlockUpdate : Packet
{
    public string command;
    public uint commandBlockMode;
    public BlockCoordinates coordinates;
    public bool isBlock;
    public bool isConditional;
    public bool isRedstoneMode;
    public string lastOutput;
    public ulong minecartEntityId;
    public string name;
    public bool shouldTrackOutput;
    public McbeCommandBlockUpdate()
    {
        Id = 0x4e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(isBlock);
        if (isBlock)
        {
            Write(coordinates);
            WriteUnsignedVarInt(commandBlockMode);
            Write(isRedstoneMode);
            Write(isConditional);
        }
        else
        {
            WriteUnsignedVarLong(minecartEntityId);
        }

        Write(command);
        Write(lastOutput);
        Write(name);
        Write(shouldTrackOutput);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        isBlock = ReadBool();
        if (isBlock)
        {
            coordinates = ReadBlockCoordinates();
            commandBlockMode = ReadUnsignedVarInt();
            isRedstoneMode = ReadBool();
            isConditional = ReadBool();
        }
        else
        {
            minecartEntityId = ReadUnsignedVarLong();
        }

        command = ReadString();
        lastOutput = ReadString();
        name = ReadString();
        shouldTrackOutput = ReadBool();
    }
}
