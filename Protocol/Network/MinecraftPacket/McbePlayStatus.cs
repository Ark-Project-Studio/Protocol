namespace Protocol.Network.MinecraftPacket;
public enum PlayStatus : Int32
{
    LoginSuccess = 0,
    LoginFailedClient = 1,
    LoginFailedServer = 2,
    PlayerSpawn = 3,
    LoginFailedInvalidTenant = 4,
    LoginFailedVanillaEdu = 5,
    LoginFailedEduVanilla = 6,
    LoginFailedServerFull = 7
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
