using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Utils;

namespace Protocol.Minecraft
{
	// Enums
	public enum CommandPermissionLevel : byte
	{
		Any = 0,
		GameDirectors = 1,
		Admin = 2,
		Host = 3,
		Owner = 4,
		Internal = 5
	}

	public enum CommandOriginType : uint
	{
		Player = 0,
		Block = 1,
		MinecartBlock = 2,
		DevConsole = 3,
		Test = 4,
		AutomationPlayer = 5,
		ClientAutomation = 6,
		DedicatedServer = 7,
		Entity = 8,
		Virtual = 9,
		GameArgument = 10,
		EntityServer = 11,
		Precompiled = 12,
		GameDirectorEntityServer = 13,
		Script = 14,
		Executor = 15
	}

	// Command Parameter Type Constants
	public static class CommandArgType
	{
		public const uint Int = 1;
		public const uint Float = 3;
		public const uint Value = 4;
		public const uint WildcardInt = 5;
		public const uint Operator = 6;
		public const uint CompareOperator = 7;
		public const uint Target = 8;
		public const uint WildcardTarget = 10;
		public const uint Filepath = 17;
		public const uint IntegerRange = 23;
		public const uint EquipmentSlots = 47;
		public const uint String = 56;
		public const uint BlockPosition = 64;
		public const uint Position = 65;
		public const uint Message = 67;
		public const uint RawText = 70;
		public const uint JSON = 74;
		public const uint BlockStates = 83;
		public const uint Command = 86;
	}

	public static class CommandArgFlag
	{
		public const uint Valid = 0x100000;
		public const uint Enum = 0x200000;
		public const uint Suffixed = 0x1000000;
		public const uint SoftEnum = 0x4000000;
	}

	public enum ParamOption : byte
	{
		CollapseEnum = 1,
		HasSemanticConstraint = 2,
		AsChainedCommand = 4
	}

	public enum CommandEnumConstraintType : byte
	{
		CheatsEnabled = 0,
		OperatorPermissions = 1,
		HostPermissions = 2
	}

	// Structures
	public struct Command
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public ushort Flags { get; set; }
		public byte PermissionLevel { get; set; }
		public uint AliasesOffset { get; set; }
		public System.Collections.Generic.List<uint> ChainedSubcommandOffsets { get; set; }
		public System.Collections.Generic.List<CommandOverload> Overloads { get; set; }
	}

	public struct CommandOverload
	{
		public bool Chaining { get; set; }
		public System.Collections.Generic.List<CommandParameter> Parameters { get; set; }
	}

	public struct CommandParameter
	{
		public string Name { get; set; }
		public uint Type { get; set; }
		public bool Optional { get; set; }
		public byte Options { get; set; }
	}

	public struct CommandEnum
	{
		public string Type { get; set; }
		public System.Collections.Generic.List<uint> ValueIndices { get; set; }
	}

	public struct ChainedSubcommand
	{
		public string Name { get; set; }
		public System.Collections.Generic.List<ChainedSubcommandValue> Values { get; set; }
	}

	public struct ChainedSubcommandValue
	{
		public uint Index { get; set; }
		public uint Value { get; set; }
	}

	public struct DynamicEnum
	{
		public string Type { get; set; }
		public System.Collections.Generic.List<string> Values { get; set; }
	}

	public struct CommandEnumConstraint
	{
		public uint EnumValueIndex { get; set; }
		public uint EnumIndex { get; set; }
		public byte[] Constraints { get; set; }
	}

	public struct CommandOrigin
	{
		public uint Origin { get; set; }
		public UUID UUID { get; set; }
		public string RequestID { get; set; }
		public long PlayerUniqueID { get; set; }
	}

	public struct CommandOutputMessage
	{
		public string Message { get; set; }
		public bool Success { get; set; }
		public System.Collections.Generic.List<string> Parameters { get; set; }
	}
}
