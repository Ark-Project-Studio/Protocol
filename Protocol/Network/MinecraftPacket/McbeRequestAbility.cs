namespace Protocol.Network.MinecraftPacket;
public class McbeRequestAbility : Packet
{
    public int ability;
    public object Value = false;
    public McbeRequestAbility()
    {
        Id = 0xb8;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteVarInt(ability);
        switch (Value)
        {
            case bool boolean:
            {
                Write((byte)1);
                Write(boolean);
                Write(0f);
                break;
            }

            case float floatingPoint:
            {
                Write((byte)2);
                Write(false);
                Write(floatingPoint);
                break;
            }
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ability = ReadVarInt();
        var type = ReadByte();
        var boolValue = ReadBool();
        var floatValue = ReadFloat();
        switch (type)
        {
            case 1:
                Value = boolValue;
                break;
            case 2:
                Value = floatValue;
                break;
        }
    }
}
