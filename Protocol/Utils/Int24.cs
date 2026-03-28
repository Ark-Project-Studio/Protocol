
using System;

namespace Protocol.Utils;

public struct Int24 : IComparable
{
	private readonly int _value;

	public Int24(ReadOnlySpan<byte> value)
	{
		_value = ToInt24(value).IntValue();
	}

	public Int24(int value)
	{
		_value = value;
	}

	private static Int24 ToInt24(ReadOnlySpan<byte> value)
	{
		if (value.Length > 3) throw new ArgumentOutOfRangeException();
		return new Int24(value[0] | (value[1] << 8) | (value[2] << 16));
	}

	public byte[] GetBytes()
	{
		return FromInt(_value);
	}

	public int IntValue()
	{
		return _value;
	}

	public static byte[] FromInt(int value)
	{
		var buffer = new byte[3];
		buffer[0] = (byte)value;
		buffer[1] = (byte)(value >> 8);
		buffer[2] = (byte)(value >> 16);
		return buffer;
	}

	public static byte[] FromInt24(Int24 value)
	{
		var buffer = new byte[3];
		buffer[0] = (byte)value.IntValue();
		buffer[1] = (byte)(value.IntValue() >> 8);
		buffer[2] = (byte)(value.IntValue() >> 16);
		return buffer;
	}

#pragma warning disable CS8767
	public int CompareTo(object value)
#pragma warning restore CS8767
	{
		return _value.CompareTo(value);
	}

	public static explicit operator Int24(byte[] values)
	{
		return new Int24(values);
	}

	public static implicit operator Int24(int value)
	{
		return new Int24(value);
	}

	public static explicit operator byte[](Int24 d)
	{
		return d.GetBytes();
	}

	public static implicit operator int(Int24 d)
	{
		return d.IntValue();
	}

	public override string ToString()
	{
		return _value.ToString();
	}
}