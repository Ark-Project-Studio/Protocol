using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeUpdateAdventureSettings : Packet
{
    public bool autoJump;
    public bool immutableWorld;
    public bool noMvp;
    public bool noPvm;
    public bool showNametags;
    public McbeUpdateAdventureSettings()
    {
        Id = 0xbc;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(noPvm);
        Write(noMvp);
        Write(immutableWorld);
        Write(showNametags);
        Write(autoJump);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        noPvm = ReadBool();
        noMvp = ReadBool();
        immutableWorld = ReadBool();
        showNametags = ReadBool();
        autoJump = ReadBool();
    }
}
