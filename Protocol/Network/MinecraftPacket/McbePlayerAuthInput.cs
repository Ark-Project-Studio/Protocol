using System.Numerics;
using Protocol.Minecraft;
using Protocol.Minecraft.Transaction;
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
    public Item OldItem { get; }
    public Item NewItem { get; }

    public InventoryAction(uint sourceType, int windowID, uint sourceFlags, uint inventorySlot, Item oldItem, Item newItem)
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
    public Item HeldItem { get; }
    public Vector3 Position { get; }
    public Vector3 ClickedPosition { get; }
    public uint BlockRuntimeID { get; }
    public uint ClientPrediction { get; }
	public byte ClientCooldownState { get; }

	public UseItemTransactionData(int legacyRequestID, LegacySetItemSlot[] legacySetItemSlots, InventoryAction[] actions, uint actionType, uint triggerType, BlockCoordinates blockPosition, int blockFace, int hotBarSlot, Item heldItem, Vector3 position, Vector3 clickedPosition, uint blockRuntimeID, uint clientPrediction,byte clientCooldownState)
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
    public enum InputFlags
    {
        InputFlagAscend,
        InputFlagDescend,
        InputFlagNorthJump,
        InputFlagJumpDown,
        InputFlagSprintDown,
        InputFlagChangeHeight,
        InputFlagJumping,
        InputFlagAutoJumpingInWater,
        InputFlagSneaking,
        InputFlagSneakDown,
        InputFlagUp,
        InputFlagDown,
        InputFlagLeft,
        InputFlagRight,
        InputFlagUpLeft,
        InputFlagUpRight,
        InputFlagWantUp,
        InputFlagWantDown,
        InputFlagWantDownSlow,
        InputFlagWantUpSlow,
        InputFlagSprinting,
        InputFlagAscendBlock,
        InputFlagDescendBlock,
        InputFlagSneakToggleDown,
        InputFlagPersistSneak,
        InputFlagStartSprinting,
        InputFlagStopSprinting,
        InputFlagStartSneaking,
        InputFlagStopSneaking,
        InputFlagStartSwimming,
        InputFlagStopSwimming,
        InputFlagStartJumping,
        InputFlagStartGliding,
        InputFlagStopGliding,
        InputFlagPerformItemInteraction,
        InputFlagPerformBlockActions,
        InputFlagPerformItemStackRequest,
        InputFlagHandledTeleport,
        InputFlagEmoting,
        InputFlagMissedSwing,
        InputFlagStartCrawling,
        InputFlagStopCrawling,
        InputFlagStartFlying,
        InputFlagStopFlying,
        InputFlagClientAckServerData,
        InputFlagClientPredictedVehicle,
        InputFlagPaddlingLeft,
        InputFlagPaddlingRight,
        InputFlagBlockBreakingDelayEnabled,
        InputFlagHorizontalCollision,
        InputFlagVerticalCollision,
        InputFlagDownLeft,
        InputFlagDownRight,
        InputFlagStartUsingItem,
        InputFlagCameraRelativeMovementEnabled,
        InputFlagRotControlledByMoveDirection,
        InputFlagStartSpinAttack,
        InputFlagStopSpinAttack,
        InputFlagIsHotbarTouchOnly,
        InputFlagJumpReleasedRaw,
        InputFlagJumpPressedRaw,
        InputFlagJumpCurrentRaw,
        InputFlagSneakReleasedRaw,
        InputFlagSneakPressedRaw,
        InputFlagSneakCurrentRaw
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
    public ItemStackRequests ItemStack = new();
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
        if (InputData.Load((int)InputFlags.InputFlagPerformItemInteraction))
            WriteUseItemTransactionData(ItemInteractionData);
        if (InputData.Load((int)InputFlags.InputFlagPerformItemStackRequest))
            Write(ItemStack);
        if (InputData.Load((int)InputFlags.InputFlagPerformBlockActions))
        {
            WriteSignedVarInt(PlayerBlockAction_?.Length ?? 0);
            if (PlayerBlockAction_ != null)
                foreach (var action in PlayerBlockAction_)
                    WritePlayerBlockAction(action);
        }

        if (InputData.Load((int)InputFlags.InputFlagClientPredictedVehicle))
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
        if (InputData.Load((int)InputFlags.InputFlagPerformItemInteraction))
            ItemInteractionData = ReadUseItemTransactionData();
        if (InputData.Load((int)InputFlags.InputFlagPerformItemStackRequest))
            ItemStack = ReadItemStackRequests(true);
        else
            ItemStack = new ItemStackRequests();
        if (InputData.Load((int)InputFlags.InputFlagPerformBlockActions))
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

        if (InputData.Load((int)InputFlags.InputFlagClientPredictedVehicle))
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

    private void WriteUseItemTransactionData(UseItemTransactionData data)
    {
        WriteSignedVarInt(data.LegacyRequestID);
        WriteSignedVarInt(data.LegacySetItemSlots?.Length ?? 0);
        if (data.LegacySetItemSlots != null)
            foreach (var slot in data.LegacySetItemSlots)
                WriteLegacySetItemSlot(slot);
        WriteSignedVarInt(data.Actions?.Length ?? 0);
        if (data.Actions != null)
            foreach (var action in data.Actions)
                WriteInventoryAction(action);
        WriteUnsignedVarInt(data.ActionType);
        WriteUnsignedVarInt(data.TriggerType);
        Write(data.BlockPosition);
        WriteSignedVarInt(data.BlockFace);
        WriteSignedVarInt(data.HotBarSlot);
        Write(data.HeldItem);
        Write(data.Position);
        Write(data.ClickedPosition);
        WriteUnsignedVarInt(data.BlockRuntimeID);
        WriteUnsignedVarInt(data.ClientPrediction);
        Write(data.ClientCooldownState);
    }

    private UseItemTransactionData ReadUseItemTransactionData()
    {
        var legacyRequestID = ReadSignedVarInt();
        var legacySlotsCount = ReadSignedVarInt();
        var legacySlots = new LegacySetItemSlot[legacySlotsCount];
        for (var i = 0; i < legacySlotsCount; i++)
            legacySlots[i] = ReadLegacySetItemSlot();
        var actionsCount = ReadSignedVarInt();
        var actions = new InventoryAction[actionsCount];
        for (var i = 0; i < actionsCount; i++)
            actions[i] = ReadInventoryAction();
        var actionType = ReadUnsignedVarInt();
        var triggerType = ReadUnsignedVarInt();
        var blockPosition = ReadBlockCoordinates();
        var blockFace = ReadSignedVarInt();
        var hotBarSlot = ReadSignedVarInt();
        var heldItem = ReadItem();
        var position = ReadVector3();
        var clickedPosition = ReadVector3();
        var blockRuntimeID = ReadUnsignedVarInt();
        var clientPrediction = ReadUnsignedVarInt();
        var clientcooldownstate = ReadByte();
        return new UseItemTransactionData(legacyRequestID, legacySlots, actions, actionType, triggerType, blockPosition, blockFace, hotBarSlot, heldItem, position, clickedPosition, blockRuntimeID, clientPrediction,clientcooldownstate);
    }

    private void WriteLegacySetItemSlot(LegacySetItemSlot slot)
    {
        Write(slot.ContainerID);
        WriteUnsignedVarInt((uint)(slot.Slots?.Length ?? 0));
        if (slot.Slots != null)
            foreach (var s in slot.Slots)
                Write(s);
    }

    private LegacySetItemSlot ReadLegacySetItemSlot()
    {
        var containerID = ReadByte();
        var slotsCount = ReadUnsignedVarInt();
        var slots = new byte[slotsCount];
        for (var i = 0; i < slotsCount; i++)
            slots[i] = ReadByte();
        return new LegacySetItemSlot(containerID, slots);
    }

    private void WriteInventoryAction(InventoryAction action)
    {
        WriteUnsignedVarInt(action.SourceType);
        WriteSignedVarInt(action.WindowID);
        WriteUnsignedVarInt(action.SourceFlags);
        WriteUnsignedVarInt(action.InventorySlot);
        Write(action.OldItem);
        Write(action.NewItem);
    }

    private InventoryAction ReadInventoryAction()
    {
        var sourceType = ReadUnsignedVarInt();
        var windowID = ReadSignedVarInt();
        var sourceFlags = ReadUnsignedVarInt();
        var inventorySlot = ReadUnsignedVarInt();
        var oldItem = ReadItem();
        var newItem = ReadItem();
        return new InventoryAction(sourceType, windowID, sourceFlags, inventorySlot, oldItem, newItem);
    }

    private void WritePlayerBlockAction(PlayerBlockAction action)
    {
        WriteSignedVarInt(action.Action);
        Write(action.BlockPos);
        WriteSignedVarInt(action.Face);
    }

    private PlayerBlockAction ReadPlayerBlockAction()
    {
        var action = ReadSignedVarInt();
        var blockPos = ReadBlockCoordinates();
        var face = ReadSignedVarInt();
        return new PlayerBlockAction(action, blockPos, face);
    }
}
