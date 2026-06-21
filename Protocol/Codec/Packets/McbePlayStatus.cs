using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum PlayStatus : Int32
{
    LoginSuccess = 0,
    LoginFailedClientOld = 1,
    LoginFailedServerOld = 2,
    PlayerSpawn = 3,
    LoginFailedInvalidTenant = 4,
    LoginFailedEditionMismatchEduToVanilla = 5,
    LoginFailedEditionMismatchVanillaToEdu = 6,
    LoginFailedServerFullSubClient = 7,
    LoginFailedEditorMismatchEditorToVanilla = 8,
    LoginFailedEditorMismatchVanillaToEditor = 9
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
