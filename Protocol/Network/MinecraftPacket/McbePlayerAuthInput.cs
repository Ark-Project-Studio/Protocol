using System.Numerics;
using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Inventory.Transaction;
using Protocol.Minecraft.Level.Block;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public readonly struct LegacySetItemSlot
{
    public byte ContainerID { get; }
    public byte[] Slots { get; }

    public LegacySetItemSlot(byte containerID, byte[] slots)
    {
        ContainerID = containerID;
        Slots = slots ?? Array.Empty<byte>();
    }
}

public readonly struct InventoryAction
{
    public uint SourceType { get; }
    public int WindowID { get; }
    public uint SourceFlags { get; }
    public uint InventorySlot { get; }
    public NetworkItemStackDescriptor OldItem { get; }
    public NetworkItemStackDescriptor NewItem { get; }

    public InventoryAction(uint sourceType, int windowID, uint sourceFlags, uint inventorySlot, NetworkItemStackDescriptor oldItem, NetworkItemStackDescriptor newItem)
    {
        SourceType = sourceType;
        WindowID = windowID;
        SourceFlags = sourceFlags;
        InventorySlot = inventorySlot;
        OldItem = oldItem;
        NewItem = newItem;
    }
}

public readonly struct UseItemTransactionData
{
    public int LegacyRequestID { get; }
    public LegacySetItemSlot[] LegacySetItemSlots { get; }
    public InventoryAction[] Actions { get; }
    public uint ActionType { get; }
    public uint TriggerType { get; }
    public BlockCoordinates BlockPosition { get; }
    public int BlockFace { get; }
    public int HotBarSlot { get; }
    public NetworkItemStackDescriptor HeldItem { get; }
    public Vector3 Position { get; }
    public Vector3 ClickedPosition { get; }
    public uint BlockRuntimeID { get; }
    public uint ClientPrediction { get; }
	public byte ClientCooldownState { get; }

    public UseItemTransactionData(int legacyRequestID, LegacySetItemSlot[] legacySetItemSlots, InventoryAction[] actions, uint actionType, uint triggerType, BlockCoordinates blockPosition, int blockFace, int hotBarSlot, NetworkItemStackDescriptor heldItem, Vector3 position, Vector3 clickedPosition, uint blockRuntimeID, uint clientPrediction,byte clientCooldownState)
    {
        LegacyRequestID = legacyRequestID;
        LegacySetItemSlots = legacySetItemSlots ?? Array.Empty<LegacySetItemSlot>();
        Actions = actions ?? Array.Empty<InventoryAction>();
        ActionType = actionType;
        TriggerType = triggerType;
        BlockPosition = blockPosition;
        BlockFace = blockFace;
        HotBarSlot = hotBarSlot;
        HeldItem = heldItem;
        Position = position;
        ClickedPosition = clickedPosition;
        BlockRuntimeID = blockRuntimeID;
        ClientPrediction = clientPrediction;
         ClientCooldownState = clientCooldownState;
    }
}

public struct PlayerBlockAction
{
    public int Action { get; set; }
    public BlockCoordinates BlockPos { get; set; }
    public int Face { get; set; }

    public PlayerBlockAction(int action, BlockCoordinates blockPos, int face)
    {
        Action = action;
        BlockPos = blockPos;
        Face = face;
    }
}

public class McbePlayerAuthInput : Packet
{
    public enum PlayerAuthInputData
    {
        Ascend = 0,
        Descend = 1,
        NorthJumpDeprecated = 2,
        JumpDown = 3,
        SprintDown = 4,
        ChangeHeight = 5,
        Jumping = 6,
        AutoJumpingInWater = 7,
        Sneaking = 8,
        SneakDown = 9,
        Up = 10,
        Down = 11,
        Left = 12,
        Right = 13,
        UpLeft = 14,
        UpRight = 15,
        WantUp = 16,
        WantDown = 17,
        WantDownSlow = 18,
        WantUpSlow = 19,
        Sprinting = 20,
        AscendBlock = 21,
        DescendBlock = 22,
        SneakToggleDown = 23,
        PersistSneak = 24,
        StartSprinting = 25,
        StopSprinting = 26,
        StartSneaking = 27,
        StopSneaking = 28,
        StartSwimming = 29,
        StopSwimming = 30,
        StartJumping = 31,
        StartGliding = 32,
        StopGliding = 33,
        PerformItemInteraction = 34,
        PerformBlockActions = 35,
        PerformItemStackRequest = 36,
        HandledTeleport = 37,
        Emoting = 38,
        MissedSwing = 39,
        StartCrawling = 40,
        StopCrawling = 41,
        StartFlying = 42,
        StopFlying = 43,
        ClientAckServerData = 44,
        IsInClientPredictedVehicle = 45,
        PaddlingLeft = 46,
        PaddlingRight = 47,
        BlockBreakingDelayEnabled = 48,
        HorizontalCollision = 49,
        VerticalCollision = 50,
        DownLeft = 51,
        DownRight = 52,
        StartUsingItem = 53,
        IsCameraRelativeMovementEnabled = 54,
        IsRotControlledByMoveDirection = 55,
        StartSpinAttack = 56,
        StopSpinAttack = 57,
        IsHotbarOnlyTouch = 58,
        JumpReleasedRaw = 59,
        JumpPressedRaw = 60,
        JumpCurrentRaw = 61,
        SneakReleasedRaw = 62,
        SneakPressedRaw = 63,
        SneakCurrentRaw = 64,
        InputNum = 65
    }

    public enum InputModes
    {
        InputModeMouse = 1,
        InputModeTouch,
        InputModeGamePad,
    }

    public enum InteractionModels
    {
        InteractionModelTouch,
        InteractionModelCrosshair,
        InteractionModelClassic
    }

    public enum PlayModes
    {
        PlayModeNormal,
        PlayModeTeaser,
        PlayModeScreen,
        PlayModeViewer,
        PlayModeReality,
        PlayModePlacement,
        PlayModeLivingRoom,
        PlayModeExitLevel,
        PlayModeExitLevelLivingRoom,
        PlayModeNumModes
    }

    public const int PlayerAuthInputBitsetSize = 65;
    public float HeadYaw;
    public Bitset InputData;
    public UseItemTransactionData ItemInteractionData;
    public ItemStackActionList[] ItemStack = [];
    public Vector2 MoveVector;
    public float Pitch, Yaw;
    public PlayerBlockAction[] PlayerBlockAction_;
    public Vector3 Position;
    public McbePlayerAuthInput()
    {
        Id = 0x90;
        IsMcbe = true;
    }

    public uint InputMode { get; set; }
    public uint PlayMode { get; set; }
    public uint InteractionModel { get; set; }
    public float InteractPitch { get; set; }
    public float InteractYaw { get; set; }
    public ulong Tick { get; set; }
    public Vector3 Delta { get; set; }
    public Vector2 VehicleRotation { get; set; }
    public long ClientPredictedVehicle { get; set; }
    public Vector2 AnalogueMoveVector { get; set; }
    public Vector3 CameraOrientation { get; set; }
    public Vector2 RawMoveVector { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Pitch);
        Write(Yaw);
        Write(Position);
        Write(MoveVector);
        Write(HeadYaw);
        WriteBitset(InputData, PlayerAuthInputBitsetSize);
        WriteUnsignedVarInt(InputMode);
        WriteUnsignedVarInt(PlayMode);
        WriteUnsignedVarInt(InteractionModel);
        Write(InteractPitch);
        Write(InteractYaw);
        WriteUnsignedVarLong(Tick);
        Write(Delta);
        if (InputData.Load((int)PlayerAuthInputData.PerformItemInteraction))
            Write(ItemInteractionData);
        if (InputData.Load((int)PlayerAuthInputData.PerformItemStackRequest))
            Write(ItemStack);
        if (InputData.Load((int)PlayerAuthInputData.PerformBlockActions))
        {
            WriteSignedVarInt(PlayerBlockAction_?.Length ?? 0);
            if (PlayerBlockAction_ != null)
                foreach (var action in PlayerBlockAction_)
                    Write(action);
        }

        if (InputData.Load((int)PlayerAuthInputData.IsInClientPredictedVehicle))
        {
            Write(VehicleRotation);
            WriteSignedVarLong(ClientPredictedVehicle);
        }

        Write(AnalogueMoveVector);
        Write(CameraOrientation);
        Write(RawMoveVector);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Pitch = ReadFloat();
        Yaw = ReadFloat();
        Position = ReadVector3();
        MoveVector = ReadVector2();
        HeadYaw = ReadFloat();
        InputData = ReadBitset(PlayerAuthInputBitsetSize);
        InputMode = ReadUnsignedVarInt();
        PlayMode = ReadUnsignedVarInt();
        InteractionModel = ReadUnsignedVarInt();
        InteractPitch = ReadFloat();
        InteractYaw = ReadFloat();
        Tick = ReadUnsignedVarLong();
        Delta = ReadVector3();
        if (InputData.Load((int)PlayerAuthInputData.PerformItemInteraction))
            ItemInteractionData = ReadUseItemTransactionData();
        if (InputData.Load((int)PlayerAuthInputData.PerformItemStackRequest))
            ItemStack = ReadItemStackRequests(true);
        else
            ItemStack = [];
        if (InputData.Load((int)PlayerAuthInputData.PerformBlockActions))
        {
            var blockActionsCount = ReadSignedVarInt();
            PlayerBlockAction_ = new PlayerBlockAction[blockActionsCount];
            for (var i = 0; i < blockActionsCount; i++)
                PlayerBlockAction_[i] = ReadPlayerBlockAction();
        }
        else
        {
            PlayerBlockAction_ = Array.Empty<PlayerBlockAction>();
        }

        if (InputData.Load((int)PlayerAuthInputData.IsInClientPredictedVehicle))
        {
            VehicleRotation = ReadVector2();
            ClientPredictedVehicle = ReadSignedVarLong();
        }
        else
        {
            VehicleRotation = Vector2.Zero;
            ClientPredictedVehicle = 0;
        }

        AnalogueMoveVector = ReadVector2();
        CameraOrientation = ReadVector3();
        RawMoveVector = ReadVector2();
    }


}
