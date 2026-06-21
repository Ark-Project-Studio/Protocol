using fNbt;

namespace Protocol.Minecraft.NBT;

public class CompoundTag
{
	public NbtCompound Value { get; set; }

	public CompoundTag()
		: this(new NbtCompound(string.Empty))
	{
	}

	public CompoundTag(NbtCompound value)
	{
		Value = value ?? new NbtCompound(string.Empty);
		Value.Name = string.Empty;
	}

	public Nbt ToNbt()
	{
		return new Nbt
		{
			NbtFile = new NbtFile(Value)
			{
				BigEndian = false,
				UseVarInt = true
			}
		};
	}

	public static CompoundTag FromNbt(Nbt nbt)
	{
		return new CompoundTag(nbt?.NbtFile?.RootTag as NbtCompound ?? new NbtCompound(string.Empty));
	}
}
