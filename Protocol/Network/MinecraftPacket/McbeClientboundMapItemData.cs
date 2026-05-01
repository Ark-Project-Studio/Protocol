using Protocol.Minecraft;
using Protocol.Minecraft.World.Map;

namespace Protocol.Network.MinecraftPacket;

public enum ClientboundMapItemDataType : uint
{
	Invalid = 0,
	TextureUpdate = 2,
	DecorationUpdate = 4,
	Creation = 8,
	All = 14
}
public class McbeClientboundMapItemData : Packet
{
	public long MapID { get; set; }
	public uint UpdateFlags { get; set; }
	public byte Dimension { get; set; }
	public bool LockedMap { get; set; }
	public BlockCoordinates Origin { get; set; }
	public byte Scale { get; set; }
	public System.Collections.Generic.List<long> MapsIncludedIn { get; set; }
	public System.Collections.Generic.List<MapTrackedObject> TrackedObjects { get; set; }
	public System.Collections.Generic.List<MapDecoration> Decorations { get; set; }
	public int Height { get; set; }
	public int Width { get; set; }
	public int XOffset { get; set; }
	public int YOffset { get; set; }
	public System.Collections.Generic.List<System.Drawing.Color> Pixels { get; set; }
	public McbeClientboundMapItemData()
    {
        Id = 0x43;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		WriteSignedVarLong(MapID);
		WriteUnsignedVarInt(UpdateFlags);
		Write(Dimension);
		Write(LockedMap);
		Write(Origin);

		if ((UpdateFlags & (uint)ClientboundMapItemDataType.Creation) != 0)
		{
			if (MapsIncludedIn != null)
			{
				WriteSlice(MapsIncludedIn.ToArray(), WriteSignedVarLong);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		if ((UpdateFlags & ((uint)ClientboundMapItemDataType.Creation | (uint)ClientboundMapItemDataType.DecorationUpdate | (uint)ClientboundMapItemDataType.TextureUpdate)) != 0)
		{
			Write(Scale);
		}

		if ((UpdateFlags & (uint)ClientboundMapItemDataType.DecorationUpdate) != 0)
		{
			if (TrackedObjects != null)
			{
				WriteSlice(TrackedObjects.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (Decorations != null)
			{
				WriteSlice(Decorations.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		if ((UpdateFlags & (uint)ClientboundMapItemDataType.TextureUpdate) != 0)
		{
			WriteSignedVarInt(Width);
			WriteSignedVarInt(Height);
			WriteSignedVarInt(XOffset);
			WriteSignedVarInt(YOffset);

			if (Pixels != null)
			{
				WriteSlice(Pixels.ToArray(), WriteVarRGBA);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();

        MapID = ReadSignedVarLong();
        UpdateFlags = ReadUnsignedVarInt();
        Dimension = ReadByte();
        LockedMap = ReadBool();
        Origin = ReadBlockCoordinates();
		

		if ((UpdateFlags & (uint)MapUpdateFlag.Initialisation) != 0)
		{
			MapsIncludedIn = new System.Collections.Generic.List<long>(
				ReadSlice(ReadSignedVarLong));
		}

		if ((UpdateFlags & ((uint)MapUpdateFlag.Initialisation | (uint)MapUpdateFlag.Decoration | (uint)MapUpdateFlag.Texture)) != 0)
		{
			Scale = ReadByte();
		}

		if ((UpdateFlags & (uint)MapUpdateFlag.Decoration) != 0)
		{
			TrackedObjects = new System.Collections.Generic.List<MapTrackedObject>(
				ReadSlice(ReadMapTrackedObject));
			Decorations = new System.Collections.Generic.List<MapDecoration>(
				ReadSlice(ReadMapDecoration));
		}

		if ((UpdateFlags & (uint)MapUpdateFlag.Texture) != 0)
		{
			Width = ReadSignedVarInt();
			Height = ReadSignedVarInt();
			XOffset = ReadSignedVarInt();
			YOffset = ReadSignedVarInt();
			Pixels = new System.Collections.Generic.List<System.Drawing.Color>(
				ReadSlice(ReadVarRGBA));
		}
	}
}
