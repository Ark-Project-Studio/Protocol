using System;
using System.Collections.Generic;
using Protocol.Minecraft;
using Protocol.Minecraft.Inventory.Transaction;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void WriteInventoryTransactionPacket(McbeInventoryTransaction packet)
		{
			WriteSignedVarInt(packet.LegacyRequestRawId);

			if (ShouldWriteLegacySetItemSlots(packet.LegacyRequestRawId))
			{
				WriteUnsignedVarInt((uint)packet.LegacySetItemSlots.Count);
				foreach (var slot in packet.LegacySetItemSlots)
				{
					Write(slot.ContainerId);
					WriteUnsignedVarInt((uint)slot.Slots.Count);
					foreach (var value in slot.Slots)
					{
						Write(value);
					}
				}
			}

			WriteUnsignedVarInt((uint)packet.TransactionType);
			WriteInventoryTransactionActions(packet.InventoryTransactionActions);
			WriteInventoryTransactionData(packet.TransactionData);
		}

		public void ReadInventoryTransactionPacket(McbeInventoryTransaction packet)
		{
			packet.LegacyRequestRawId = ReadSignedVarInt();
			packet.LegacySetItemSlots.Clear();

			if (ShouldWriteLegacySetItemSlots(packet.LegacyRequestRawId))
			{
				uint count = ReadUnsignedVarInt();
				for (int i = 0; i < count; i++)
				{
					var slot = new InventoryTransactionLegacySetItemSlot { ContainerId = ReadByte() };
					uint slotCount = ReadUnsignedVarInt();
					for (int j = 0; j < slotCount; j++)
					{
						slot.Slots.Add(ReadByte());
					}
					packet.LegacySetItemSlots.Add(slot);
				}
			}

			packet.TransactionType = (McbeInventoryTransaction.InventoryTransactionType)ReadUnsignedVarInt();
			packet.InventoryTransactionActions = ReadInventoryTransactionActions();
			packet.TransactionData = ReadInventoryTransactionData(packet.TransactionType);
		}

		public void WriteInventoryTransactionActions(List<InventoryTransactionAction> actions)
		{
			WriteUnsignedVarInt((uint)(actions?.Count ?? 0));
			if (actions == null) return;

			foreach (var action in actions)
			{
				WriteInventoryTransactionAction(action);
			}
		}

		public List<InventoryTransactionAction> ReadInventoryTransactionActions()
		{
			var actions = new List<InventoryTransactionAction>();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				actions.Add(ReadInventoryTransactionAction());
			}
			return actions;
		}

		public void WriteInventoryTransactionAction(InventoryTransactionAction action)
		{
			WriteInventoryTransactionSource(action);
			WriteUnsignedVarInt(action.InventorySlot);
			Write(action.FromItem);
			Write(action.ToItem);
		}

		public InventoryTransactionAction ReadInventoryTransactionAction()
		{
			var action = ReadInventoryTransactionSource();
			action.InventorySlot = ReadUnsignedVarInt();
			action.FromItem = ReadNetworkItemStackDescriptor();
			action.ToItem = ReadNetworkItemStackDescriptor();
			return action;
		}

		public void WriteInventoryTransactionSource(InventoryTransactionAction action)
		{
			WriteUnsignedVarInt((uint)action.SourceType);
			switch (action.SourceType)
			{
				case McbeInventoryTransaction.InventoryTransactionSourceType.ContainerInventory:
				case McbeInventoryTransaction.InventoryTransactionSourceType.NonImplementedFeatureTODO:
					WriteSignedVarInt(action.ContainerId);
					break;
				case McbeInventoryTransaction.InventoryTransactionSourceType.WorldInteraction:
					WriteUnsignedVarInt(action.BitFlags);
					break;
			}
		}

		public InventoryTransactionAction ReadInventoryTransactionSource()
		{
			var action = new InventoryTransactionAction
			{
				SourceType = (McbeInventoryTransaction.InventoryTransactionSourceType)ReadUnsignedVarInt()
			};

			switch (action.SourceType)
			{
				case McbeInventoryTransaction.InventoryTransactionSourceType.ContainerInventory:
				case McbeInventoryTransaction.InventoryTransactionSourceType.NonImplementedFeatureTODO:
					action.ContainerId = ReadSignedVarInt();
					break;
				case McbeInventoryTransaction.InventoryTransactionSourceType.WorldInteraction:
					action.BitFlags = ReadUnsignedVarInt();
					break;
			}

			return action;
		}

		public void WriteInventoryTransactionData(InventoryTransactionData data)
		{
			switch (data)
			{
				case ItemUseInventoryTransactionData itemUse:
					WriteUnsignedVarInt((uint)itemUse.ActionType);
					WriteUnsignedVarInt((uint)itemUse.TriggerType);
					Write(itemUse.Position);
					WriteSignedVarInt(itemUse.Face);
					WriteSignedVarInt(itemUse.Slot);
					Write(itemUse.Item);
					Write(itemUse.FromPosition);
					Write(itemUse.ClickPosition);
					WriteUnsignedVarInt(itemUse.TargetBlockRuntimeId);
					Write((byte)itemUse.ClientPredictedResult);
					Write((byte)itemUse.ClientCooldownState);
					break;
				case ItemUseOnActorInventoryTransactionData itemUseOnActor:
					WriteUnsignedVarLong(itemUseOnActor.RuntimeId);
					WriteUnsignedVarInt((uint)itemUseOnActor.ActionType);
					WriteSignedVarInt(itemUseOnActor.Slot);
					Write(itemUseOnActor.Item);
					Write(itemUseOnActor.FromPosition);
					Write(itemUseOnActor.HitPosition);
					break;
				case ItemReleaseInventoryTransactionData itemRelease:
					WriteUnsignedVarInt((uint)itemRelease.ActionType);
					WriteSignedVarInt(itemRelease.Slot);
					Write(itemRelease.Item);
					Write(itemRelease.FromPosition);
					break;
			}
		}

		public InventoryTransactionData ReadInventoryTransactionData(McbeInventoryTransaction.InventoryTransactionType type)
		{
			return type switch
			{
				McbeInventoryTransaction.InventoryTransactionType.InventoryMismatch => new InventoryMismatchTransactionData(),
				McbeInventoryTransaction.InventoryTransactionType.ItemUseInventoryTransaction => new ItemUseInventoryTransactionData
				{
					ActionType = (McbeInventoryTransaction.ItemUseActionType)ReadUnsignedVarInt(),
					TriggerType = (McbeInventoryTransaction.TriggerType)ReadUnsignedVarInt(),
					Position = ReadBlockCoordinates(),
					Face = ReadSignedVarInt(),
					Slot = ReadSignedVarInt(),
					Item = ReadNetworkItemStackDescriptor(),
					FromPosition = ReadVector3(),
					ClickPosition = ReadVector3(),
					TargetBlockRuntimeId = ReadUnsignedVarInt(),
					ClientPredictedResult = (McbeInventoryTransaction.PredictedResult)ReadByte(),
					ClientCooldownState = (McbeInventoryTransaction.ClientCooldownState)ReadByte()
				},
				McbeInventoryTransaction.InventoryTransactionType.ItemUseOnActorInventoryTransaction => new ItemUseOnActorInventoryTransactionData
				{
					RuntimeId = ReadUnsignedVarLong(),
					ActionType = (McbeInventoryTransaction.ItemUseOnActorActionType)ReadUnsignedVarInt(),
					Slot = ReadSignedVarInt(),
					Item = ReadNetworkItemStackDescriptor(),
					FromPosition = ReadVector3(),
					HitPosition = ReadVector3()
				},
				McbeInventoryTransaction.InventoryTransactionType.ItemReleaseInventoryTransaction => new ItemReleaseInventoryTransactionData
				{
					ActionType = (McbeInventoryTransaction.ItemReleaseActionType)ReadUnsignedVarInt(),
					Slot = ReadSignedVarInt(),
					Item = ReadNetworkItemStackDescriptor(),
					FromPosition = ReadVector3()
				},
				_ => new InventoryNormalTransactionData()
			};
		}

		private static bool ShouldWriteLegacySetItemSlots(int legacyRequestRawId)
		{
			return legacyRequestRawId < -1 && (legacyRequestRawId & 1) == 0;
		}

		public StackRequestSlotInfo ReadStackRequestSlotInfo()
		{
			var containerName = readFullContainerName();
			var slot = (byte)ReadByte();
			var stackNetworkId = ReadSignedVarInt();


			return new StackRequestSlotInfo()
			{
				FullContainerName = containerName,
				Slot = slot,
				StackNetworkId = stackNetworkId
			};
		}

		public FullContainerName readFullContainerName()
		{
			var name = new FullContainerName();
			name.DynamicContainerID = new Optional<uint>();
			name.ContainerID = ReadByte();
			var readBool = ReadBool();
			if (readBool)
			{
				name.DynamicContainerID.HasValue = true;
				name.DynamicContainerID.Value = ReadUint();
			}
			else
			{
				name.DynamicContainerID.HasValue = readBool;
			}

			return name;
		}

		public void Write(FullContainerName name)
		{
			Write(name.ContainerID);
			Write(name.DynamicContainerID.HasValue);
			if (name.DynamicContainerID.HasValue)
			{
				Write(name.DynamicContainerID.Value);
			}
		}

		public void Write(StackRequestSlotInfo slotInfo)
		{
			Write(slotInfo.FullContainerName);
			Write(slotInfo.Slot);
			WriteSignedVarInt(slotInfo.StackNetworkId);
		}

		public void Write(ItemStackActionList[] requests)
		{
			WriteSlice(requests ?? [], request =>
			{
				WriteSignedVarInt(request.RequestId);
				WriteSlice(request.Actions, action =>
				{
					Write((byte)action.ActionType);
					Write(action);
				});

				WriteUnsignedVarInt((uint)request.StringsToFilter.Count);
				foreach (var value in request.StringsToFilter)
				{
					Write(value);
				}

				Write(request.StringsToFilterOrigin);
			});
		}

		private void Write(ItemStackAction action)
		{
			switch (action.ActionType)
			{
				case ItemStackRequestActionType.Take:
				case ItemStackRequestActionType.Place:
					Write(action.Amount);
					Write(action.Source);
					Write(action.Destination);
					break;
				case ItemStackRequestActionType.Swap:
					Write(action.Source);
					Write(action.Destination);
					break;
				case ItemStackRequestActionType.Drop:
					Write(action.Amount);
					Write(action.Source);
					Write(action.Randomly);
					break;
				case ItemStackRequestActionType.Destroy:
				case ItemStackRequestActionType.Consume:
					Write(action.Amount);
					Write(action.Source);
					break;
				case ItemStackRequestActionType.Create:
					Write(action.ResultsIndex);
					break;
				case ItemStackRequestActionType.BeaconPayment:
					WriteVarInt(action.PrimaryEffectId);
					WriteVarInt(action.SecondaryEffectId);
					break;
				case ItemStackRequestActionType.MineBlock:
					WriteVarInt(action.Slot);
					WriteVarInt(action.PredictedDurability);
					if (action.PreValidationStatus == MineBlockPreValidationStatus.Valid && action.ItemStackNetId != 0)
					{
						WriteVarInt(action.ItemStackNetId);
					}
					break;
				case ItemStackRequestActionType.CraftRecipe:
				case ItemStackRequestActionType.CraftCreative:
					WriteUnsignedVarInt(action.RecipeNetworkIdOrCreativeId);
					WriteUnsignedVarInt(action.TimesCraftedVarInt);
					break;
				case ItemStackRequestActionType.CraftRecipeAuto:
					WriteUnsignedVarInt(action.RecipeNetworkIdOrCreativeId);
					Write(action.NumberOfRequestedCrafts);
					Write(action.TimesCrafted);
					Write((byte)action.Ingredients.Count);
					foreach (var ingredient in action.Ingredients)
					{
						Write(ingredient);
					}
					break;
				case ItemStackRequestActionType.CraftRecipeOptional:
					WriteUnsignedVarInt(action.RecipeNetId);
					Write(action.FilteredStringIndex);
					break;
				case ItemStackRequestActionType.CraftGrindStone:
					WriteUnsignedVarInt((uint)action.ItemStackNetId);
					Write(action.TimesCrafted);
					WriteVarInt(action.RepairCost);
					break;
				case ItemStackRequestActionType.CraftLoom:
					Write(action.PatternNameId);
					Write(action.TimesCrafted);
					break;
				case ItemStackRequestActionType.CraftResults:
					Write(action.CraftResults);
					Write(action.TimesCrafted);
					break;
			}
		}

		public ItemStackActionList[] ReadItemStackRequests(bool single = false)
		{
			uint c = 1;

			if (!single)
			{
				c = ReadUnsignedVarInt();
			}


			var requests = new ItemStackActionList[c];
			for (int i = 0; i < c; i++)
			{
				var actions = new ItemStackActionList();
				actions.RequestId = ReadSignedVarInt();


				uint count = ReadUnsignedVarInt();
				actions.Actions = new ItemStackAction[count];

				for (int j = 0; j < count; j++)
				{
					var action = new ItemStackAction { ActionType = (ItemStackRequestActionType)ReadByte() };

					switch (action.ActionType)
					{
						case ItemStackRequestActionType.Take:
						case ItemStackRequestActionType.Place:
						{
							action.Amount = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							break;
						}
						case ItemStackRequestActionType.Swap:
						{
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							break;
						}
						case ItemStackRequestActionType.Drop:
						{
							action.Amount = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Randomly = ReadBool();
							break;
						}
						case ItemStackRequestActionType.Destroy:
						case ItemStackRequestActionType.Consume:
						{
							action.Amount = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							break;
						}
						case ItemStackRequestActionType.Create:
						{
							action.ResultsIndex = ReadByte();
							break;
						}
						case ItemStackRequestActionType.BeaconPayment:
						{
							action.PrimaryEffectId = ReadVarInt();
							action.SecondaryEffectId = ReadVarInt();
							break;
						}
						case ItemStackRequestActionType.MineBlock:
						{
							action.Slot = ReadVarInt();
							action.PredictedDurability = ReadVarInt();
							action.ItemStackNetId = ReadVarInt();
							action.PreValidationStatus = action.ItemStackNetId == 0 ? MineBlockPreValidationStatus.Invalid : MineBlockPreValidationStatus.Valid;
							break;
						}
						case ItemStackRequestActionType.CraftRecipe:
						case ItemStackRequestActionType.CraftCreative:
						{
							action.RecipeNetworkIdOrCreativeId = ReadUnsignedVarInt();
							action.TimesCraftedVarInt = ReadUnsignedVarInt();
							break;
						}
						case ItemStackRequestActionType.CraftRecipeAuto:
						{
							action.RecipeNetworkIdOrCreativeId = ReadUnsignedVarInt();
							action.NumberOfRequestedCrafts = ReadByte();
							action.TimesCrafted = ReadByte();
							var cou = ReadByte();
							for (var a = 0; a < cou; a++)
							{
								action.Ingredients.Add(ReadRecipeIngredient());
							}
							break;
						}
						case ItemStackRequestActionType.CraftRecipeOptional:
						{
							action.RecipeNetId = ReadUnsignedVarInt();
							action.FilteredStringIndex = ReadUint();
							break;
						}
						case ItemStackRequestActionType.CraftGrindStone:
						{
							action.ItemStackNetId = (int)ReadUnsignedVarInt();
							action.TimesCrafted = ReadByte();
							action.RepairCost = ReadVarInt();
							break;
						}
						case ItemStackRequestActionType.CraftLoom:
						{
							action.PatternNameId = ReadString();
							action.TimesCrafted = ReadByte();
							break;
						}
						case ItemStackRequestActionType.CraftResults:
						{
							action.CraftResults = ReadNetworkItemInstanceDescriptors();
							action.TimesCrafted = ReadByte();
							break;
						}
						default:
							break;
					}

					actions.Actions[j] = action;
				}

				requests[i] = actions;

				var filterStringCount = ReadUnsignedVarInt();

				for (int fi = 0; fi < filterStringCount; fi++)
				{
					actions.StringsToFilter.Add(ReadString());
				}

				actions.StringsToFilterOrigin = ReadInt();
			}

			return requests;
		}

		public void Write(ItemStackResponse[] responses)
		{
			WriteSlice(responses ?? [], stackResponse =>
			{
				Write((byte)stackResponse.Result);
				WriteSignedVarInt(stackResponse.RequestId);
				if (stackResponse.Result == StackResponseStatus.Ok)
				{
					WriteUnsignedVarInt((uint)stackResponse.ResponseContainerInfos.Count);
					foreach (StackResponseContainerInfo containerInfo in stackResponse.ResponseContainerInfos)
					{
						Write(containerInfo.ContainerName);
						WriteUnsignedVarInt((uint)containerInfo.Slots.Count);
						foreach (StackResponseSlotInfo slot in containerInfo.Slots)
						{
							Write(slot.RequestedSlot);
							Write(slot.Slot);
							Write(slot.Count);
							WriteSignedVarInt(slot.StackNetworkId);
							Write(slot.CustomName);
							Write(slot.FilteredCustomName);
							WriteSignedVarInt(slot.DurationCorrection);
						}
					}
				}
			});
		}


		public ItemStackResponse[] ReadItemStackResponses()
		{
			return ReadSlice(() =>
			{
				var response = new ItemStackResponse();
				response.Result = (StackResponseStatus)ReadByte();
				response.RequestId = ReadSignedVarInt();

				if (response.Result != StackResponseStatus.Ok)
					return response;

				response.ResponseContainerInfos = new List<StackResponseContainerInfo>();
				var subCount = ReadUnsignedVarInt();
				for (int sub = 0; sub < subCount; sub++)
				{
					var containerInfo = new StackResponseContainerInfo();
					containerInfo.ContainerName = readFullContainerName();
					var slotCount = ReadUnsignedVarInt();
					containerInfo.Slots = new List<StackResponseSlotInfo>();

					for (int si = 0; si < slotCount; si++)
					{
						var slot = new StackResponseSlotInfo();
						slot.RequestedSlot = ReadByte();
						slot.Slot = ReadByte();
						slot.Count = ReadByte();
						slot.StackNetworkId = ReadSignedVarInt();
						slot.CustomName = ReadString();
						slot.FilteredCustomName = ReadString();
						slot.DurationCorrection = ReadSignedVarInt();

						containerInfo.Slots.Add(slot);
					}

					response.ResponseContainerInfos.Add(containerInfo);
				}

				return response;
			});
		}
	}
}
