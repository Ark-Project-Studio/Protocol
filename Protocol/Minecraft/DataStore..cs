using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft
{
	public enum DataStoreChangeType
	{
		Update = 0,
		Change = 1,
		Removal = 2
	}

	public enum DataStoreControl
	{
		Double = 0,
		Boolean = 1,
		String = 2
	}

	public enum DataStorePropertyType
	{
		None = 0,
		Bool = 1,
		Int64 = 2,
		String = 4,
		Map = 6
	}

	// Structures
	public struct DataStorePropertyValue
	{
		public int Type { get; set; }
		public bool BoolValue { get; set; }
		public long Int64Value { get; set; }
		public string StringValue { get; set; }
		public System.Collections.Generic.List<DataStoreMapEntry> MapValue { get; set; }
	}

	public struct DataStoreMapEntry
	{
		public string Key { get; set; }
		public DataStorePropertyValue Value { get; set; }
	}

	public struct DataStoreUpdate
	{
		public string DataStoreName { get; set; }
		public string Property { get; set; }
		public string Path { get; set; }
		public uint ControlType { get; set; }
		public double DoubleValue { get; set; }
		public bool BoolValue { get; set; }
		public string StringValue { get; set; }
		public uint PropertyUpdateCount { get; set; }
		public uint PathUpdateCount { get; set; }
	}

	public struct DataStoreChange
	{
		public string DataStoreName { get; set; }
		public string Property { get; set; }
		public uint UpdateCount { get; set; }
		public DataStorePropertyValue NewValue { get; set; }
	}

	public struct DataStoreRemoval
	{
		public string DataStoreName { get; set; }
	}

	public struct DataStoreChangeEntry
	{
		public uint ChangeType { get; set; }
		public DataStoreUpdate Update { get; set; }
		public DataStoreChange Change { get; set; }
		public DataStoreRemoval Removal { get; set; }
	}
}
