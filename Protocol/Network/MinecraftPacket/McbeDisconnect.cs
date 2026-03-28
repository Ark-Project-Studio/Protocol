namespace Protocol.Network.MinecraftPacket;
public class McbeDisconnect : Packet
{
    public enum DisconnectReason
    {
        Unknown = 0,
        CantConnectNoInternet,
        NoPermissions,
        UnrecoverableError,
        ThirdPartyBlocked,
        ThirdPartyNoInternet,
        ThirdPartyBadIP,
        ThirdPartyNoServerOrServerLocked,
        VersionMismatch,
        SkinIssue,
        InviteSessionNotFound,
        EduLevelSettingsMissing,
        LocalServerNotFound,
        LegacyDisconnect,
        UserLeaveGameAttempted,
        PlatformLockedSkinsError,
        RealmsWorldUnassigned,
        RealmsServerCantConnect,
        RealmsServerHidden,
        RealmsServerDisabledBeta,
        RealmsServerDisabled,
        CrossPlatformDisabled,
        CantConnect,
        SessionNotFound,
        ServerFull = 27,
        InvalidPlatformSkin,
        EditionVersionMismatch,
        EditionMismatch,
        LevelNewerThanExeVersion,
        NoFailOccurred,
        BannedSkin,
        Timeout,
        ServerNotFound,
        OutdatedServer,
        OutdatedClient,
        MultiplayerDisabled = 39,
        NoWiFi,
        NoReason = 42,
        Disconnected,
        InvalidPlayer,
        LoggedInOtherLocation,
        ServerIdConflict,
        NotAllowed,
        NotAuthenticated,
        InvalidTenant,
        UnknownPacket,
        UnexpectedPacket,
        InvalidCommandRequestPacket,
        HostSuspended,
        LoginPacketNoRequest,
        LoginPacketNoCert,
        MissingClient,
        Kicked,
        KickedForExploit,
        KickedForIdle,
        ResourcePackProblem,
        IncompatiblePack,
        OutOfStorage,
        InvalidLevel,
        BlockMismatch = 68,
        InvalidHeights,
        InvalidWidths,
        Shutdown = 73,
        LoadingStateTimeout = 75,
        ResourcePackLoadingFailed,
        SearchingForSessionLoadingScreenFailed,
        NetherNetProtocolVersion,
        SubsystemStatusError,
        EmptyAuthFromDiscovery,
        EmptyUrlFromDiscovery,
        ExpiredAuthFromDiscovery,
        UnknownSignalServiceSignInFailure,
        XBLJoinLobbyFailure,
        UnspecifiedClientInstanceDisconnection,
        NetherNetSessionNotFound,
        NetherNetCreatePeerConnection,
        NetherNetICE,
        NetherNetConnectRequest,
        NetherNetConnectResponse,
        NetherNetNegotiationTimeout,
        NetherNetInactivityTimeout,
        StaleConnectionBeingReplaced,
        BadPacket = 98,
        NetherNetFailedToCreateOffer,
        NetherNetFailedToCreateAnswer,
        NetherNetFailedToSetLocalDescription,
        NetherNetFailedToSetRemoteDescription,
        NetherNetNegotiationTimeoutWaitingForResponse,
        NetherNetNegotiationTimeoutWaitingForAccept,
        NetherNetIncomingConnectionIgnored,
        NetherNetSignalingParsingFailure,
        NetherNetSignalingUnknownError,
        NetherNetSignalingUnicastDeliveryFailed,
        NetherNetSignalingBroadcastDeliveryFailed,
        NetherNetSignalingGenericDeliveryFailed,
        EditorMismatchEditorWorld,
        EditorMismatchVanillaWorld,
        WorldTransferNotPrimaryClient,
        RequestServerShutdown,
        ClientGameSetupCancelled,
        ClientGameSetupFailed,
        NetherNetSignalingSigninFailed = 120,
        SessionAccessDenied,
        ServiceSigninIssue,
        NetherNetNoSignalingChannel,
        NetherNetNotLoggedIn,
        NetherNetClientSignalingError,
        SubClientLoginDisabled,
        DeepLinkTryingToOpenDemoWorldWhileSignedIn,
        AsyncJoinTaskDenied,
        RealmsTimelineRequired,
        GuestWithoutHost,
        FailedToJoinExperience,
        NetherNetDataChannelClosed
    }

    public DisconnectReason failReason;
    public string filteredMessage;
    public bool hideDisconnectReason;
    public string message;
    public McbeDisconnect()
    {
        Id = 0x05;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(0);
        Write(hideDisconnectReason);
        Write(message);
        Write(filteredMessage);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        failReason = (DisconnectReason)ReadUnsignedVarInt();
        hideDisconnectReason = ReadBool();
        message = ReadString();
        filteredMessage = ReadString();
    }
}
