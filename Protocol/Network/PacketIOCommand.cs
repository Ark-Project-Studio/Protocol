using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;
using Protocol.Minecraft.Command;

namespace Protocol.Network
{
	public partial class Packet
	{
		// Write methods
		public void Write(Command value)
		{
			string permissionStr = CommandPermissionToString(value.PermissionLevel);
			Write(value.Name);
			Write(value.Description);
			Write(value.Flags);
			Write(permissionStr);
			Write(value.AliasesOffset);

			if (value.ChainedSubcommandOffsets != null)
			{
				WriteSlice(value.ChainedSubcommandOffsets.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.Overloads != null)
			{
				WriteSlice(value.Overloads.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		public void Write(CommandOverload value)
		{
			Write(value.Chaining);

			if (value.Parameters != null)
			{
				WriteSlice(value.Parameters.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		public void Write(CommandParameter value)
		{
			Write(value.Name);
			Write(value.Type);
			Write(value.Optional);
			Write(value.Options);
		}

		public void Write(CommandEnum value, System.Collections.Generic.List<string> enumValues)
		{
			Write(value.Type);

			if (value.ValueIndices != null)
			{
				WriteSlice(value.ValueIndices.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		public void Write(ChainedSubcommand value)
		{
			Write(value.Name);

			if (value.Values != null)
			{
				WriteSlice(value.Values.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		public void Write(ChainedSubcommandValue value)
		{
			WriteUnsignedVarInt(value.Index);
			WriteUnsignedVarInt(value.Value);
		}

		public void Write(DynamicEnum value)
		{
			Write(value.Type);

			if (value.Values != null)
			{
				WriteSlice(value.Values.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		public void Write(CommandEnumConstraint value)
		{
			Write(value.EnumValueIndex);
			Write(value.EnumIndex);
			WriteByteArray(value.Constraints);
		}

		public void Write(CommandOrigin value)
		{
			string originStr = CommandOriginToString(value.Origin);
			Write(originStr);
			Write(value.UUID);
			Write(value.RequestID);
			Write(value.PlayerUniqueID);
		}

		public void Write(CommandOutputMessage value)
		{
			Write(value.Message);
			Write(value.Success);

			if (value.Parameters != null)
			{
				WriteSlice(value.Parameters.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}
		// Read methods
		public Command ReadCommand()
		{
			var value = new Command
			{
				Name = ReadString(),
				Description = ReadString(),
				Flags = ReadUshort()
			};

			string permissionStr = ReadString();
			value.PermissionLevel = CommandPermissionFromString(permissionStr);
			value.AliasesOffset = ReadUint();

			value.ChainedSubcommandOffsets = new System.Collections.Generic.List<uint>(ReadSlice(ReadUint));
			value.Overloads = new System.Collections.Generic.List<CommandOverload>(ReadSlice(ReadCommandOverload));

			return value;
		}

		public CommandOverload ReadCommandOverload()
		{
			return new CommandOverload
			{
				Chaining = ReadBool(),
				Parameters = new System.Collections.Generic.List<CommandParameter>(ReadSlice(ReadCommandParameter))
			};
		}

		public CommandParameter ReadCommandParameter()
		{
			return new CommandParameter
			{
				Name = ReadString(),
				Type = ReadUint(),
				Optional = ReadBool(),
				Options = ReadByte()
			};
		}

		public CommandEnum ReadCommandEnum(System.Collections.Generic.List<string> enumValues)
		{
			return new CommandEnum
			{
				Type = ReadString(),
				ValueIndices = new System.Collections.Generic.List<uint>(ReadSlice(ReadUint))
			};
		}

		public ChainedSubcommand ReadChainedSubcommand()
		{
			return new ChainedSubcommand
			{
				Name = ReadString(),
				Values = new System.Collections.Generic.List<ChainedSubcommandValue>(ReadSlice(ReadChainedSubcommandValue))
			};
		}

		public ChainedSubcommandValue ReadChainedSubcommandValue()
		{
			return new ChainedSubcommandValue
			{
				Index = ReadUnsignedVarInt(),
				Value = ReadUnsignedVarInt()
			};
		}

		public DynamicEnum ReadDynamicEnum()
		{
			return new DynamicEnum
			{
				Type = ReadString(),
				Values = new System.Collections.Generic.List<string>(ReadSlice(ReadString))
			};
		}

		public CommandEnumConstraint ReadCommandEnumConstraint()
		{
			return new CommandEnumConstraint
			{
				EnumValueIndex = ReadUint(),
				EnumIndex = ReadUint(),
				Constraints = ReadByteArray(true)
			};
		}

		public CommandOrigin ReadCommandOrigin()
		{
			string originStr = ReadString();
			return new CommandOrigin
			{
				Origin = CommandOriginFromString(originStr),
				UUID = ReadUUID(),
				RequestID = ReadString(),
				PlayerUniqueID = ReadLong()
			};
		}

		public CommandOutputMessage ReadCommandOutputMessage()
		{
			return new CommandOutputMessage
			{
				Message = ReadString(),
				Success = ReadBool(),
				Parameters = new System.Collections.Generic.List<string>(ReadSlice(ReadString))
			};
		}
		// Helper methods for enum conversions
		private string CommandPermissionToString(byte permission)
		{
			switch (permission)
			{
				case (byte)CommandPermissionLevel.Any: return "any";
				case (byte)CommandPermissionLevel.GameDirectors: return "gamedirectors";
				case (byte)CommandPermissionLevel.Admin: return "admin";
				case (byte)CommandPermissionLevel.Host: return "host";
				case (byte)CommandPermissionLevel.Owner: return "owner";
				case (byte)CommandPermissionLevel.Internal: return "internal";
				default: return "unknown";
			}
		}

		private byte CommandPermissionFromString(string s)
		{
			switch (s)
			{
				case "any": return (byte)CommandPermissionLevel.Any;
				case "gamedirectors": return (byte)CommandPermissionLevel.GameDirectors;
				case "admin": return (byte)CommandPermissionLevel.Admin;
				case "host": return (byte)CommandPermissionLevel.Host;
				case "owner": return (byte)CommandPermissionLevel.Owner;
				case "internal": return (byte)CommandPermissionLevel.Internal;
				default: throw new FormatException($"Unknown permission level: {s}");
			}
		}

		private string CommandOriginToString(uint origin)
		{
			switch (origin)
			{
				case (uint)CommandOriginType.Player: return "player";
				case (uint)CommandOriginType.CommandBlock: return "commandblock";
				case (uint)CommandOriginType.MinecartCommandBlock: return "minecartcommandblock";
				case (uint)CommandOriginType.DevConsole: return "devconsole";
				case (uint)CommandOriginType.Test: return "test";
				case (uint)CommandOriginType.AutomationPlayer: return "automationplayer";
				case (uint)CommandOriginType.ClientAutomation: return "clientautomation";
				case (uint)CommandOriginType.DedicatedServer: return "dedicatedserver";
				case (uint)CommandOriginType.Entity: return "entity";
				case (uint)CommandOriginType.Virtual: return "virtual";
				case (uint)CommandOriginType.GameArgument: return "gameargument";
				case (uint)CommandOriginType.EntityServer: return "entityserver";
				case (uint)CommandOriginType.Precompiled: return "precompiled";
				case (uint)CommandOriginType.GameDirectorEntityServer: return "gamedirectorentityserver";
				case (uint)CommandOriginType.Scripting: return "scripting";
				case (uint)CommandOriginType.ExecuteContext: return "executecontext";
				default: return "unknown";
			}
		}

		private uint CommandOriginFromString(string s)
		{
			switch (s)
			{
				case "player": return (uint)CommandOriginType.Player;
				case "commandblock": return (uint)CommandOriginType.CommandBlock;
				case "minecartcommandblock": return (uint)CommandOriginType.MinecartCommandBlock;
				case "devconsole": return (uint)CommandOriginType.DevConsole;
				case "test": return (uint)CommandOriginType.Test;
				case "automationplayer": return (uint)CommandOriginType.AutomationPlayer;
				case "clientautomation": return (uint)CommandOriginType.ClientAutomation;
				case "dedicatedserver": return (uint)CommandOriginType.DedicatedServer;
				case "entity": return (uint)CommandOriginType.Entity;
				case "virtual": return (uint)CommandOriginType.Virtual;
				case "gameargument": return (uint)CommandOriginType.GameArgument;
				case "entityserver": return (uint)CommandOriginType.EntityServer;
				case "precompiled": return (uint)CommandOriginType.Precompiled;
				case "gamedirectorentityserver": return (uint)CommandOriginType.GameDirectorEntityServer;
				case "scripting": return (uint)CommandOriginType.Scripting;
				case "executecontext": return (uint)CommandOriginType.ExecuteContext;
				default: throw new FormatException($"Unknown origin: {s}");
			}
		}
	}
}
