using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network
{
	public partial class Packet 
	{
		public unsafe void WriteSlice<T>(T[] array, Action<T> action)
		{
			WriteUnsignedVarInt((uint)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}

		public unsafe void WriteSlice<T>(T[] array, Action<T, bool> action, bool bigendian)
		{
			WriteUnsignedVarInt((uint)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i], bigendian);
			}
		}

		public T[] ReadSlice<T>(Func<T> func)
		{
			uint count = ReadUnsignedVarInt();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func();
			}
			return result;
		}

		public T[] ReadSlice<T>(Func<bool, T> func, bool bigendian)
		{
			uint count = ReadUnsignedVarInt();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func(bigendian);
			}
			return result;
		}

		public void WriteSliceUint8Length<T>(T[] array, Action<T> action)
		{
			Write((byte)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}

		public void WriteSliceUint8Length<T>(T[] array, Action<T, bool> action, bool bigendian)
		{
			Write((byte)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i], bigendian);
			}
		}

		public T[] ReadSliceUint8Length<T>(Func<T> func)
		{
			byte count = ReadByte();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func();
			}
			return result;
		}

		public T[] ReadSliceUint8Length<T>(Func<bool, T> func, bool bigendian)
		{
			byte count = ReadByte();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func(bigendian);
			}
			return result;
		}

		public void WriteSliceUint16Length<T>(T[] array, Action<T> action)
		{
			Write((ushort)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}

		public void WriteSliceUint16Length<T>(T[] array, Action<T, bool> action, bool bigendian)
		{
			Write((ushort)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i], bigendian);
			}
		}

		public T[] ReadSliceUint16Length<T>(Func<T> func)
		{
			ushort count = ReadUshort();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func();
			}
			return result;
		}

		public T[] ReadSliceUint16Length<T>(Func<bool, T> func, bool bigendian)
		{
			ushort count = ReadUshort();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func(bigendian);
			}
			return result;
		}

		public void WriteSliceUint32Length<T>(T[] array, Action<T> action)
		{
			Write((uint)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}

		public void WriteSliceUint32Length<T>(T[] array, Action<T, bool> action, bool bigendian)
		{
			Write((uint)array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i], bigendian);
			}
		}

		public T[] ReadSliceUint32Length<T>(Func<T> func)
		{
			uint count = ReadUint();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func();
			}
			return result;
		}

		public T[] ReadSliceUint32Length<T>(Func<bool, T> func, bool bigendian)
		{
			uint count = ReadUint();
			if (count == 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func(bigendian);
			}
			return result;
		}

		public void WriteSliceVarint32Length<T>(T[] array, Action<T> action)
		{
			WriteSignedVarInt(array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i]);
			}
		}

		public void WriteSliceVarint32Length<T>(T[] array, Action<T, bool> action, bool bigendian)
		{
			WriteSignedVarInt(array.Length);

			for (int i = 0; i < array.Length; i++)
			{
				action(array[i], bigendian);
			}
		}

		public T[] ReadSliceVarint32Length<T>(Func<T> func)
		{
			int count = ReadSignedVarInt();
			if (count <= 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func();
			}
			return result;
		}

		public T[] ReadSliceVarint32Length<T>(Func<bool, T> func, bool bigendian)
		{
			int count = ReadSignedVarInt();
			if (count <= 0) return Array.Empty<T>();

			var result = new T[count];
			for (int i = 0; i < count; i++)
			{
				result[i] = func(bigendian);
			}
			return result;
		}

		public void WriteSliceOfLen<T>(T[] array,int length, Action<T> action)
		{
			for (int i = 0; i < length; i++)
			{
				action(array[i]);
			}
		}

		public T[] ReadSliceOfLen<T>( int length, Func<T> action)
		{
			T[] array = new T[length];	
			for (int i = 0; i < length; i++)
			{
				array[i] = action();
			}

			return array;
		}
		public void WriteSliceOfLen<T>(T[] array, int length, Action<T,bool> action,bool bigendian)
		{
			for (int i = 0; i < length; i++)
			{
				action(array[i],bigendian);
			}
		}

		public T[] ReadSliceOfLen<T>(int length, Func<bool,T> action,bool bigendian)
		{
			T[] array = new T[length];
			for (int i = 0; i < length; i++)
			{
				array[i] = action(bigendian);
			}

			return array;
		}

	}
}
