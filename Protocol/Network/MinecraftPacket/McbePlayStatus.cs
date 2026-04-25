namespace Protocol.Network.MinecraftPacket;
public enum PlayStatus : Int32
{
    LoginSuccess = 0,
    LoginFailed_ClientOld = 1,
    LoginFailed_ServerOld = 2,
    PlayerSpawn = 3,
    LoginFailed_InvalidTenant = 4,
    LoginFailed_EditionMismatchEduToVanilla = 5,
    LoginFailed_EditionMismatchVanillaToEdu = 6,
    LoginFailed_ServerFullSubClient = 7,
    LoginFailed_EditorMismatchEditorToVanilla = 8,
    LoginFailed_EditorMismatchVanillaToEditor = 9
}

public class McbePlayStatus : Packet
{
    public PlayStatus status;
    public McbePlayStatus()
    {
        Id = 0x02;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteBe((Int32)status);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        status = (PlayStatus)ReadIntBe();
    }
}
