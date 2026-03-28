using System.Numerics;
using Protocol.Minecraft;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public struct ServerJoinInformation
{
    public Optional<GatheringJoinInfo> GatheringJoinInfo { get; set; }
}

public struct GatheringJoinInfo
{
    public string ExperienceID { get; set; }
    public string ExperienceName { get; set; }
    public string ExperienceWorldID { get; set; }
    public string ExperienceWorldName { get; set; }
    public string CreatorID { get; set; }
    public string StoreID { get; set; }
}

public class SpawnSettings
{
    public short BiomeType { get; set; }
    public string BiomeName { get; set; }
    public int Dimension { get; set; }

    public void Read(Packet packet)
    {
        BiomeType = packet.ReadShort();
        BiomeName = packet.ReadString();
        Dimension = packet.ReadVarInt();
    }

    public void Write(Packet packet)
    {
        packet.Write(BiomeType);
        packet.Write(BiomeName);
        packet.WriteVarInt(Dimension);
    }
}

public class LevelSettings
{
    public bool bonusChest;
    public bool broadcastToLan;
    public byte chatRestrictionLevel;
    public bool createdInEditorMode;
    public int difficulty;
    public bool editorWorld;
    public int eduOffer;
    public string eduProductUuid;
    public EducationUriResource eduSharedUriResource;
    public bool emoteChatMuted;
    public bool enableCommands;
    public bool experimentalGameplayOverride;
    public Experiments experiments;
    public bool exportedFromEditorMode;
    public int gamemode;
    public List<GameRule> gamerules;
    public string gameVersion;
    public int generator;
    public bool hardcoreEnabled;
    public bool hasAchievementsDisabled;
    public bool hasConfirmedPlatformLockedContent;
    public bool hasEduFeaturesEnabled;
    public bool hasLockedBehaviorPack;
    public bool hasLockedResourcePack;
    public bool isDisablePlayerInteractions;
    public bool isDisablingCustomSkins;
    public bool isDisablingPersonas;
    public bool isFromLockedWorldTemplate;
    public bool isFromWorldTemplate;
    public bool isMultiplayer;
    public bool isNewNether;
    public bool isTexturepacksRequired;
    public bool isWorldTemplateOptionLocked;
    public float lightningLevel;
    public int limitedWorldLength;
    public int limitedWorldWidth;
    public bool mapEnabled;
    public bool onlySpawnV1Villagers;
    public byte permissionLevel;
    public int platformBroadcastMode;
    public float rainLevel;
    public long seed;
    public int serverChunkTickRange;
    public SpawnSettings spawnSettings;
    public int time;
    public bool useMsaGamertagsOnly;
    public int x;
    public int xboxLiveBroadcastMode;
    public int y;
    public int z;
    public void Write(Packet packet)
    {
        packet.Write(seed);
        var s = spawnSettings ?? new SpawnSettings();
        s.Write(packet);
        packet.WriteSignedVarInt(generator);
        packet.WriteSignedVarInt(gamemode);
        packet.Write(false);
        packet.WriteSignedVarInt(difficulty);
        packet.WriteSignedVarInt(x);
        packet.WriteVarInt(y);
        packet.WriteSignedVarInt(z);
        packet.Write(hasAchievementsDisabled);
        packet.Write(editorWorld);
        packet.Write(createdInEditorMode);
        packet.Write(exportedFromEditorMode);
        packet.WriteSignedVarInt(time);
        packet.WriteSignedVarInt(eduOffer);
        packet.Write(hasEduFeaturesEnabled);
        packet.Write(eduProductUuid);
        packet.Write(rainLevel);
        packet.Write(lightningLevel);
        packet.Write(hasConfirmedPlatformLockedContent);
        packet.Write(isMultiplayer);
        packet.Write(broadcastToLan);
        packet.WriteVarInt(xboxLiveBroadcastMode);
        packet.WriteVarInt(platformBroadcastMode);
        packet.Write(enableCommands);
        packet.Write(isTexturepacksRequired);
        packet.WriteSlice(gamerules.ToArray(), packet.WriteGameRuleLegacy);
        packet.Write(experiments);
        packet.Write(false);
        packet.Write(bonusChest);
        packet.Write(mapEnabled);
        packet.Write(permissionLevel);
        packet.Write(serverChunkTickRange);
        packet.Write(hasLockedBehaviorPack);
        packet.Write(hasLockedResourcePack);
        packet.Write(isFromLockedWorldTemplate);
        packet.Write(useMsaGamertagsOnly);
        packet.Write(isFromWorldTemplate);
        packet.Write(isWorldTemplateOptionLocked);
        packet.Write(onlySpawnV1Villagers);
        packet.Write(isDisablingPersonas);
        packet.Write(isDisablingCustomSkins);
        packet.Write(emoteChatMuted);
        packet.Write(gameVersion);
        packet.Write(limitedWorldWidth);
        packet.Write(limitedWorldLength);
        packet.Write(isNewNether);
        packet.Write(eduSharedUriResource ?? new EducationUriResource("", ""));
        packet.Write(false);
        packet.Write(chatRestrictionLevel);
        packet.Write(isDisablePlayerInteractions);
    }

    public void Read(Packet packet)
    {
        seed = packet.ReadLong();
        spawnSettings = new SpawnSettings();
        spawnSettings.Read(packet);
        generator = packet.ReadSignedVarInt();
        gamemode = packet.ReadSignedVarInt();
        hardcoreEnabled = packet.ReadBool();
        difficulty = packet.ReadSignedVarInt();
        x = packet.ReadSignedVarInt();
        y = packet.ReadVarInt();
        z = packet.ReadSignedVarInt();
        hasAchievementsDisabled = packet.ReadBool();
        editorWorld = packet.ReadBool();
        createdInEditorMode = packet.ReadBool();
        exportedFromEditorMode = packet.ReadBool();
        time = packet.ReadSignedVarInt();
        eduOffer = packet.ReadSignedVarInt();
        hasEduFeaturesEnabled = packet.ReadBool();
        eduProductUuid = packet.ReadString();
        rainLevel = packet.ReadFloat();
        lightningLevel = packet.ReadFloat();
        hasConfirmedPlatformLockedContent = packet.ReadBool();
        isMultiplayer = packet.ReadBool();
        broadcastToLan = packet.ReadBool();
        xboxLiveBroadcastMode = packet.ReadVarInt();
        platformBroadcastMode = packet.ReadVarInt();
        enableCommands = packet.ReadBool();
        isTexturepacksRequired = packet.ReadBool();
        gamerules = packet.ReadSlice(() =>
        {
	        return packet.ReadGameRuleLegacy();
        }).ToList();
        experiments = packet.ReadExperiments();
        packet.ReadBool();
        bonusChest = packet.ReadBool();
        mapEnabled = packet.ReadBool();
        permissionLevel = packet.ReadByte();
        serverChunkTickRange = packet.ReadInt();
        hasLockedBehaviorPack = packet.ReadBool();
        hasLockedResourcePack = packet.ReadBool();
        isFromLockedWorldTemplate = packet.ReadBool();
        useMsaGamertagsOnly = packet.ReadBool();
        isFromWorldTemplate = packet.ReadBool();
        isWorldTemplateOptionLocked = packet.ReadBool();
        onlySpawnV1Villagers = packet.ReadBool();
        isDisablingPersonas = packet.ReadBool();
        isDisablingCustomSkins = packet.ReadBool();
        emoteChatMuted = packet.ReadBool();
        gameVersion = packet.ReadString();
        limitedWorldWidth = packet.ReadInt();
        limitedWorldLength = packet.ReadInt();
        isNewNether = packet.ReadBool();
        eduSharedUriResource = packet.ReadEducationUriResource();
        if (packet.ReadBool())
            experimentalGameplayOverride = packet.ReadBool();
        else
            experimentalGameplayOverride = false;
        chatRestrictionLevel = packet.ReadByte();
        isDisablePlayerInteractions = packet.ReadBool();
    }
}

public class McbeStartGame : Packet
{
    public bool blockNetworkIdsAreHashes;
    public BlockPalette blockPalette;
    public ulong blockPaletteChecksum;
    public bool clientSideGenerationEnabled;
    public long currentTick;
    public bool enableNewBlockBreakSystem;
    public bool enableNewInventorySystem;
    public int enchantmentSeed;
    public long entityIdSelf;
    public bool isTrial;
    public string levelId;
    public LevelSettings levelSettings = new();
    public int movementRewindHistorySize;
    public string multiplayerCorrelationId;
    public int playerGamemode;
    public string premiumWorldTemplateId;
    public Nbt propertyData;
    public Vector2 rotation;
    public ulong runtimeEntityId;
    public Vector3 spawn;
    public bool TickDeathSystems;
    public string worldId;
    public string worldName;
    public UUID worldTemplateId;
    public Optional<ServerJoinInformation> ServerJoinInformation;
    public string scenarioId;
    public string serverId;
    public string serverVersion;
    public string OwnerID;
    public McbeStartGame()
    {
        Id = 0x0b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(entityIdSelf);
        WriteUnsignedVarLong(runtimeEntityId);
        WriteSignedVarInt(playerGamemode);
        Write(spawn);
        Write(rotation);
        var s = levelSettings ?? new LevelSettings();
        s.Write(this);
        Write(levelId);
        Write(worldName);
        Write(premiumWorldTemplateId);
        Write(isTrial);
        WriteSignedVarInt(movementRewindHistorySize);
        Write(enableNewBlockBreakSystem);
        Write(currentTick);
        WriteSignedVarInt(enchantmentSeed);
        Write(blockPalette); //
        Write(multiplayerCorrelationId);
        Write(enableNewInventorySystem);
        Write(serverVersion);
        Write(propertyData);
        Write(blockPaletteChecksum);
        Write(worldTemplateId); //
        Write(clientSideGenerationEnabled);
        Write(blockNetworkIdsAreHashes);
        if (ServerJoinInformation.HasValue)
        {
            Write(ServerJoinInformation.Value);
        }

        Write(serverId);
        Write(scenarioId);
        Write(worldId);
        Write(OwnerID);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entityIdSelf = ReadSignedVarLong();
        runtimeEntityId = ReadUnsignedVarLong();
        playerGamemode = ReadSignedVarInt();
        spawn = ReadVector3();
        rotation = ReadVector2();
        levelSettings = new LevelSettings();
        levelSettings.Read(this);
        levelId = ReadString();
        worldName = ReadString();
        premiumWorldTemplateId = ReadString();
        isTrial = ReadBool();
        movementRewindHistorySize = ReadSignedVarInt();
        enableNewBlockBreakSystem = ReadBool();
        currentTick = ReadLong();
        enchantmentSeed = ReadSignedVarInt();
        try
        {
            blockPalette = ReadBlockPalette();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to read complete blockpallete");
        }

        multiplayerCorrelationId = ReadString();
        enableNewInventorySystem = ReadBool();
        serverVersion = ReadString();
        propertyData = ReadNbt();
        blockPaletteChecksum = ReadUlong();
        worldTemplateId = ReadUUID();
        clientSideGenerationEnabled = ReadBool();
        blockNetworkIdsAreHashes = ReadBool();
        if (ReadBool())
        {
            ServerJoinInformation = new Optional<ServerJoinInformation>(ReadServerJoinInformation());
        }

        serverId = ReadString();
        scenarioId = ReadString();
        worldId = ReadString();
        OwnerID = ReadString();
    }
}
