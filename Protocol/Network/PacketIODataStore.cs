using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;
using Protocol.Minecraft.Level;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(DataStorePropertyValue value)
		{
			Write(value.Type);

			switch (value.Type)
			{
				case (int)DataStorePropertyType.None:
					break;
				case (int)DataStorePropertyType.Bool:
					Write(value.BoolValue);
					break;
				case (int)DataStorePropertyType.Int64:
					Write(value.Int64Value);
					break;
				case (int)DataStorePropertyType.String:
					Write(value.StringValue);
					break;
				case (int)DataStorePropertyType.Map:
					if (value.MapValue != null)
					{
						WriteSliceVarint32Length(value.MapValue.ToArray(), Write);
					}
					else
					{
						WriteSignedVarInt(0);
					}
					break;
				default:
					throw new ArgumentException($"Unknown data store property type: {value.Type}");
			}
		}

		public void Write(DataStoreMapEntry value)
		{
			Write(value.Key);
			Write(value.Value);
		}

		public void Write(DataStoreUpdate value)
		{
			Write(value.DataStoreName);
			Write(value.Property);
			Write(value.Path);
			Write(value.ControlType);

			switch (value.ControlType)
			{
				case (uint)DataStoreControl.Double:
					Write(value.DoubleValue);
					break;
				case (uint)DataStoreControl.Boolean:
					Write(value.BoolValue);
					break;
				case (uint)DataStoreControl.String:
					Write(value.StringValue);
					break;
				default:
					throw new ArgumentException($"Unknown data store control type: {value.ControlType}");
			}

			Write(value.PropertyUpdateCount);
			Write(value.PathUpdateCount);
		}

		public void Write(DataStoreChange value)
		{
			Write(value.DataStoreName);
			Write(value.Property);
			Write(value.UpdateCount);
			Write(value.NewValue);
		}

		public void Write(DataStoreRemoval value)
		{
			Write(value.DataStoreName);
		}

		public void Write(DataStoreChangeEntry value)
		{
			Write(value.ChangeType);

			switch (value.ChangeType)
			{
				case (uint)DataStoreChangeType.Update:
					Write(value.Update);
					break;
				case (uint)DataStoreChangeType.Change:
					Write(value.Change.DataStoreName);
					Write(value.Change.Property);
					Write(value.Change.UpdateCount);
					Write(value.Change.NewValue);
					break;
				case (uint)DataStoreChangeType.Removal:
					Write(value.Removal.DataStoreName);
					break;
				default:
					throw new ArgumentException($"Unknown data store change type: {value.ChangeType}");
			}
		}

		// Read methods
		public DataStorePropertyValue ReadDataStorePropertyValue()
		{
			var value = new DataStorePropertyValue
			{
				Type = ReadInt()
			};

			switch (value.Type)
			{
				case (int)DataStorePropertyType.None:
					break;
				case (int)DataStorePropertyType.Bool:
					value.BoolValue = ReadBool();
					break;
				case (int)DataStorePropertyType.Int64:
					value.Int64Value = ReadLong();
					break;
				case (int)DataStorePropertyType.String:
					value.StringValue = ReadString();
					break;
				case (int)DataStorePropertyType.Map:
					var mapArray = ReadSliceVarint32Length(ReadDataStoreMapEntry);
					value.MapValue = new System.Collections.Generic.List<DataStoreMapEntry>(mapArray);
					break;
				default:
					throw new FormatException($"Unknown data store property type: {value.Type}");
			}

			return value;
		}

		public DataStoreMapEntry ReadDataStoreMapEntry()
		{
			return new DataStoreMapEntry
			{
				Key = ReadString(),
				Value = ReadDataStorePropertyValue()
			};
		}

		public DataStoreUpdate ReadDataStoreUpdate()
		{
			var update = new DataStoreUpdate
			{
				DataStoreName = ReadString(),
				Property = ReadString(),
				Path = ReadString(),
				ControlType = ReadUint()
			};

			switch (update.ControlType)
			{
				case (uint)DataStoreControl.Double:
					update.DoubleValue = ReadDouble();
					break;
				case (uint)DataStoreControl.Boolean:
					update.BoolValue = ReadBool();
					break;
				case (uint)DataStoreControl.String:
					update.StringValue = ReadString();
					break;
				default:
					throw new FormatException($"Unknown data store control type: {update.ControlType}");
			}

			update.PropertyUpdateCount = ReadUint();
			update.PathUpdateCount = ReadUint();

			return update;
		}

		public DataStoreChange ReadDataStoreChange()
		{
			return new DataStoreChange
			{
				DataStoreName = ReadString(),
				Property = ReadString(),
				UpdateCount = ReadUint(),
				NewValue = ReadDataStorePropertyValue()
			};
		}

		public DataStoreRemoval ReadDataStoreRemoval()
		{
			return new DataStoreRemoval
			{
				DataStoreName = ReadString()
			};
		}

		public DataStoreChangeEntry ReadDataStoreChangeEntry()
		{
			var entry = new DataStoreChangeEntry
			{
				ChangeType = ReadUint()
			};

			switch (entry.ChangeType)
			{
				case (uint)DataStoreChangeType.Update:
					entry.Update = ReadDataStoreUpdate();
					break;
				case (uint)DataStoreChangeType.Change:
					entry.Change = new DataStoreChange
					{
						DataStoreName = ReadString(),
						Property = ReadString(),
						UpdateCount = ReadUint(),
						NewValue = ReadDataStorePropertyValue()
					};
					break;
				case (uint)DataStoreChangeType.Removal:
					entry.Removal = new DataStoreRemoval
					{
						DataStoreName = ReadString()
					};
					break;
				default:
					throw new FormatException($"Unknown data store change type: {entry.ChangeType}");
			}

			return entry;
		}
	}
}
