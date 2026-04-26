using System;
using System.Collections.Generic;
using Protocol.Minecraft;
using Protocol.Minecraft.Transaction;
using Protocol.Network.MinecraftPacket;
using Transaction = Protocol.Minecraft.Transaction.Transaction;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(Transaction transaction)
		{
			WriteSignedVarInt(transaction.RequestId);

			if (transaction.RequestId != 0)
			{
				WriteUnsignedVarInt((uint)transaction.RequestRecords.Count);

				foreach (var record in transaction.RequestRecords)
				{
					Write(record.ContainerId);
					WriteUnsignedVarInt((uint)record.Slots.Count);

					foreach (var slot in record.Slots)
					{
						Write(slot);
					}
				}
			}

			switch (transaction)
			{
				case InventoryMismatchTransaction _:
					WriteUnsignedVarInt((int)McbeInventoryTransaction.TransactionType.InventoryMismatch);
					break;
				case ItemReleaseTransaction _:
					WriteUnsignedVarInt((int)McbeInventoryTransaction.TransactionType.ItemRelease);
					break;
				case ItemUseOnEntityTransaction _:
					WriteUnsignedVarInt((int)McbeInventoryTransaction.TransactionType.ItemUseOnEntity);
					break;
				case ItemUseTransaction _:
					WriteUnsignedVarInt((int)McbeInventoryTransaction.TransactionType.ItemUse);
					break;
				case NormalTransaction _:
					WriteUnsignedVarInt((int)McbeInventoryTransaction.TransactionType.Normal);
					break;
			}


			WriteUnsignedVarInt((uint)transaction.TransactionRecords.Count);
			foreach (var record in transaction.TransactionRecords)
			{
				switch (record)
				{
					case ContainerTransactionRecord r:
						WriteVarInt((int)McbeInventoryTransaction.InventorySourceType.Container);
						WriteSignedVarInt(r.InventoryId);
						break;
					case GlobalTransactionRecord _:
						WriteVarInt((int)McbeInventoryTransaction.InventorySourceType.Global);
						break;
					case WorldInteractionTransactionRecord r:
						WriteVarInt((int)McbeInventoryTransaction.InventorySourceType.WorldInteraction);
						WriteVarInt(r.Flags);
						break;
					case CreativeTransactionRecord _:
						WriteVarInt((int)McbeInventoryTransaction.InventorySourceType.Creative);
						break;
					case CraftTransactionRecord r:
						WriteVarInt((int)McbeInventoryTransaction.InventorySourceType.Crafting);
						WriteVarInt((int)r.Action);
						break;
				}

				WriteVarInt(record.Slot);
				Write(record.OldItem);
				Write(record.NewItem);
			}

			switch (transaction)
			{
				case NormalTransaction _:
				case InventoryMismatchTransaction _:
					break;
				case ItemUseTransaction t:
					WriteUnsignedVarInt((uint)t.ActionType);
					WriteUnsignedVarInt((uint)t.TriggerType);
					Write(t.Position);
					WriteSignedVarInt(t.Face);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					Write(t.ClickPosition);
					WriteUnsignedVarInt(t.BlockRuntimeId);
					Write(t.ClientPredictedResult);
					break;
				case ItemUseOnEntityTransaction t:
					WriteUnsignedVarLong((ulong)t.EntityId);
					WriteUnsignedVarInt((uint)t.ActionType);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					Write(t.ClickPosition);
					break;
				case ItemReleaseTransaction t:
					WriteUnsignedVarInt((uint)t.ActionType);
					WriteSignedVarInt(t.Slot);
					Write(t.Item);
					Write(t.FromPosition);
					break;
				default:
					break;
			}
		}

		public Transaction ReadTransaction()
		{
			var requestId = ReadSignedVarInt();
			var requestRecords = new List<RequestRecord>();
			if (requestId != 0)
			{
				var c1 = ReadUnsignedVarInt();
				for (int i = 0; i < c1; i++)
				{
					var rr = new RequestRecord();
					rr.ContainerId = ReadByte();
					var c2 = ReadUnsignedVarInt();
					for (int j = 0; j < c2; j++)
					{
						byte slot = ReadByte();
						rr.Slots.Add(slot);
					}

					requestRecords.Add(rr);
				}
			}

			var transactionType = (McbeInventoryTransaction.TransactionType)ReadVarInt();


			var transactions = new List<TransactionRecord>();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				TransactionRecord record;
				int sourceType = ReadVarInt();
				switch ((McbeInventoryTransaction.InventorySourceType)sourceType)
				{
					case McbeInventoryTransaction.InventorySourceType.Container:
						record = new ContainerTransactionRecord() { InventoryId = ReadSignedVarInt() };
						break;
					case McbeInventoryTransaction.InventorySourceType.Global:
						record = new GlobalTransactionRecord();
						break;
					case McbeInventoryTransaction.InventorySourceType.WorldInteraction:
						record = new WorldInteractionTransactionRecord() { Flags = ReadVarInt() };
						break;
					case McbeInventoryTransaction.InventorySourceType.Creative:
						record = new CreativeTransactionRecord() { InventoryId = 0x79 };
						break;
					case McbeInventoryTransaction.InventorySourceType.Unspecified:
					case McbeInventoryTransaction.InventorySourceType.Crafting:
						record = new CraftTransactionRecord()
							{ Action = (McbeInventoryTransaction.CraftingAction)ReadSignedVarInt() };
						break;
					default:
						Console.WriteLine($"Unknown inventory source type={sourceType}");
						continue;
				}

				record.Slot = ReadVarInt();
				record.OldItem = ReadItem();
				record.NewItem = ReadItem();


				transactions.Add(record);
			}

			Transaction transaction = null;
			switch (transactionType)
			{
				case McbeInventoryTransaction.TransactionType.Normal:
					transaction = new NormalTransaction();
					break;
				case McbeInventoryTransaction.TransactionType.InventoryMismatch:
					transaction = new InventoryMismatchTransaction();
					break;
				case McbeInventoryTransaction.TransactionType.ItemUse:
					transaction = new ItemUseTransaction()
					{
						ActionType = (McbeInventoryTransaction.ItemUseAction)ReadVarInt(),
						TriggerType = (McbeInventoryTransaction.TriggerType)ReadVarInt(),
						Position = ReadBlockCoordinates(),
						Face = ReadSignedVarInt(),
						Slot = ReadSignedVarInt(),
						Item = ReadItem(),
						FromPosition = ReadVector3(),
						ClickPosition = ReadVector3(),
						BlockRuntimeId = ReadUnsignedVarInt(),
						ClientPredictedResult = ReadUnsignedVarInt()
					};
					break;
				case McbeInventoryTransaction.TransactionType.ItemUseOnEntity:
					transaction = new ItemUseOnEntityTransaction()
					{
						EntityId = ReadVarLong(),
						ActionType = (McbeInventoryTransaction.ItemUseOnEntityAction)ReadVarInt(),
						Slot = ReadSignedVarInt(),
						Item = ReadItem(),
						FromPosition = ReadVector3(),
						ClickPosition = ReadVector3()
					};
					break;
				case McbeInventoryTransaction.TransactionType.ItemRelease:
					transaction = new ItemReleaseTransaction()
					{
						ActionType = (McbeInventoryTransaction.ItemReleaseAction)ReadVarInt(),
						Slot = ReadSignedVarInt(),
						Item = ReadItem(),
						FromPosition = ReadVector3()
					};
					break;
			}

			transaction.TransactionRecords = transactions;
			transaction.RequestId = requestId;
			transaction.RequestRecords = requestRecords;

			return transaction;
		}

		public StackRequestSlotInfo ReadStackRequestSlotInfo()
		{
			var containerName = readFullContainerName();
			var slot = (byte)ReadByte();
			var stackNetworkId = ReadSignedVarInt();


			return new StackRequestSlotInfo()
			{
				ContainerId = containerName.ContainerID,
				Slot = slot,
				StackNetworkId = stackNetworkId,
				DynamicId = (int)containerName.DynamicContainerID.Value
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
			Write(new FullContainerName()
			{
				ContainerID = slotInfo.ContainerId, DynamicContainerID = new Optional<uint>((uint)slotInfo.DynamicId)
			});
			Write(slotInfo.Slot);
			WriteSignedVarInt(slotInfo.StackNetworkId);
		}

		public void Write(ItemStackRequests requests)
		{
			WriteUnsignedVarInt((uint)requests.Count);

			foreach (ItemStackActionList request in requests)
			{
				WriteSignedVarInt(request.RequestId);
				WriteUnsignedVarInt((uint)request.Count);

				foreach (ItemStackAction action in request)
				{
					switch (action)
					{
						case TakeAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.Take);
							Write(ta.Count);
							Write(ta.Source);
							Write(ta.Destination);
							break;
						}

						case PlaceAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.Place);
							Write(ta.Count);
							Write(ta.Source);
							Write(ta.Destination);
							break;
						}

						case SwapAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.Swap);
							Write(ta.Source);
							Write(ta.Destination);
							break;
						}

						case DropAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.Drop);
							Write(ta.Count);
							Write(ta.Source);
							Write(ta.Randomly);
							break;
						}

						case DestroyAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.Destroy);
							Write(ta.Count);
							Write(ta.Source);
							break;
						}

						case ConsumeAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.Consume);
							Write(ta.Count);
							Write(ta.Source);
							break;
						}

						case CreateAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.Create);
							Write(ta.ResultSlot);
							break;
						}

						case PlaceIntoBundleAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.PlaceIntoBundleDeprecated);
							break;
						}

						case TakeFromBundleAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.TakeFromBundleDeprecated);
							break;
						}

						case LabTableCombineAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.LabTableCombine);
							break;
						}

						case BeaconPaymentAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.BeaconPayment);
							WriteSignedVarInt(ta.PrimaryEffect);
							WriteSignedVarInt(ta.SecondaryEffect);
							break;
						}

						case CraftAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftRecipe);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							Write(ta.TimesCrafted);
							break;
						}

						case CraftAutoAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftRecipeAuto);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							Write(ta.TimesCrafted2);
							Write(ta.TimesCrafted);
							Write((byte)ta.Ingredients.Count);
							foreach (Item item in ta.Ingredients)
							{
								WriteRecipeIngredient(item);
							}

							break;
						}

						case CraftCreativeAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftCreative);
							WriteUnsignedVarInt(ta.CreativeItemNetworkId);
							Write(ta.ClientPredictedResult);
							break;
						}

						case CraftRecipeOptionalAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftRecipeOptional);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							Write(ta.FilteredStringIndex);
							break;
						}

						case GrindstoneStackRequestAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftGrindstone);
							WriteUnsignedVarInt(ta.RecipeNetworkId);
							WriteVarInt(ta.RepairCost);
							Write(ta.TimesCrafted);
							break;
						}

						case LoomStackRequestAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftLoom);
							Write(ta.PatternId);
							Write(ta.TimesCrafted);
							break;
						}

						case CraftNotImplementedDeprecatedAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftNotImplementedDeprecated);
							break;
						}

						case CraftResultDeprecatedAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.CraftResultsDeprecated);
							WriteItems(ta.ResultItems);
							Write(ta.TimesCrafted);
							break;
						}

						case MineBlockAction ta:
						{
							Write((byte)McbeItemStackRequest.ActionType.MineBlock);
							WriteVarInt(ta.Slot);
							WriteVarInt(ta.Durability);
							WriteSignedVarInt(ta.stackNetworkId);
							break;
						}
					}
				}

				WriteUnsignedVarInt((uint)request.filteredString.Count);

				for (int fi = 0; fi < request.filteredString.Count; fi++)
				{
					Write(request.filteredString[fi]);
				}

				Write(request.FilterCause);
			}
		}

		public ItemStackRequests ReadItemStackRequests(bool single = false)
		{
			var requests = new ItemStackRequests();

			uint c = 1;

			if (!single)
			{
				c = ReadUnsignedVarInt();
			}


			for (int i = 0; i < c; i++)
			{
				var actions = new ItemStackActionList();
				actions.RequestId = ReadSignedVarInt();


				uint count = ReadUnsignedVarInt();

				for (int j = 0; j < count; j++)
				{
					var actionType = (McbeItemStackRequest.ActionType)ReadByte();

					switch (actionType)
					{
						case McbeItemStackRequest.ActionType.Take:
						{
							var action = new TakeAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.Place:
						{
							var action = new PlaceAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.Swap:
						{
							var action = new SwapAction();
							action.Source = ReadStackRequestSlotInfo();
							action.Destination = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.Drop:
						{
							var action = new DropAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							action.Randomly = ReadBool();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.Destroy:
						{
							var action = new DestroyAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.Consume:
						{
							var action = new ConsumeAction();
							action.Count = ReadByte();
							action.Source = ReadStackRequestSlotInfo();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.Create:
						{
							var action = new CreateAction();
							action.ResultSlot = ReadByte();
							actions.Add(action);
							break;
						}

						case McbeItemStackRequest.ActionType.PlaceIntoBundleDeprecated:
						{
							var action = new PlaceIntoBundleAction();
							actions.Add(action);
							break;
						}

						case McbeItemStackRequest.ActionType.TakeFromBundleDeprecated:
						{
							var action = new TakeFromBundleAction();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.LabTableCombine:
						{
							var action = new LabTableCombineAction();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.BeaconPayment:
						{
							var action = new BeaconPaymentAction();
							action.PrimaryEffect = ReadSignedVarInt();
							action.SecondaryEffect = ReadSignedVarInt();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftRecipe:
						{
							var action = new CraftAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							action.TimesCrafted = ReadByte();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftRecipeAuto:
						{
							var action = new CraftAutoAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							action.TimesCrafted2 = ReadByte();
							action.TimesCrafted = ReadByte();
							var cou = ReadByte();
							for (var a = 0; a < cou; a++)
							{
								action.Ingredients.Add(ReadRecipeData());
							}

							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftCreative:
						{
							var action = new CraftCreativeAction();
							action.CreativeItemNetworkId = ReadUnsignedVarInt();
							action.ClientPredictedResult = ReadByte();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftRecipeOptional:
						{
							var action = new CraftRecipeOptionalAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							action.FilteredStringIndex = ReadInt();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftGrindstone:
						{
							var action = new GrindstoneStackRequestAction();
							action.RecipeNetworkId = ReadUnsignedVarInt();
							action.RepairCost = ReadVarInt();
							action.TimesCrafted = ReadByte();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftLoom:
						{
							var action = new LoomStackRequestAction();
							action.PatternId = ReadString();
							action.TimesCrafted = ReadByte();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftNotImplementedDeprecated:
						{
							var action = new CraftNotImplementedDeprecatedAction();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.CraftResultsDeprecated:
						{
							var action = new CraftResultDeprecatedAction();
							action.ResultItems = ReadItems();
							action.TimesCrafted = ReadByte();
							actions.Add(action);
							break;
						}
						case McbeItemStackRequest.ActionType.MineBlock:
						{
							var action = new MineBlockAction();
							action.Slot = ReadVarInt();
							action.Durability = ReadVarInt();
							action.stackNetworkId = ReadSignedVarInt();
							actions.Add(action);
							break;
						}
						default:
							throw new ArgumentOutOfRangeException();
					}
				}

				requests.Add(actions);

				var filterStringCount = ReadUnsignedVarInt();

				for (int fi = 0; fi < filterStringCount; fi++)
				{
					actions.filteredString.Add(ReadString());
				}

				var filterStringCause = ReadUint();
			}

			return requests;
		}

		public void Write(ItemStackResponses responses)
		{
			WriteUnsignedVarInt((uint)responses.Count);
			foreach (ItemStackResponse stackResponse in responses)
			{
				Write((byte)stackResponse.Result);
				WriteSignedVarInt(stackResponse.RequestId);
				if (stackResponse.Result != StackResponseStatus.Ok)
					continue;
				WriteUnsignedVarInt((uint)stackResponse.ResponseContainerInfos.Count);
				foreach (StackResponseContainerInfo containerInfo in stackResponse.ResponseContainerInfos)
				{
					Write(new FullContainerName()
					{
						ContainerID = containerInfo.ContainerId,
						DynamicContainerID = new Optional<uint>((uint)containerInfo.DynamicId)
					});
					WriteUnsignedVarInt((uint)containerInfo.Slots.Count);
					foreach (StackResponseSlotInfo slot in containerInfo.Slots)
					{
						Write(slot.Slot);
						Write(slot.HotbarSlot);
						Write(slot.Count);
						WriteSignedVarInt(slot.StackNetworkId);
						Write(slot.CustomName);
						Write(slot.FilteredCustomName);
						WriteSignedVarInt(slot.DurabilityCorrection);
					}
				}
			}
		}


		public ItemStackResponses ReadItemStackResponses()
		{
			var responses = new ItemStackResponses();
			var count = ReadUnsignedVarInt();

			for (var i = 0; i < count; i++)
			{
				var response = new ItemStackResponse();
				response.Result = (StackResponseStatus)ReadByte();
				response.RequestId = ReadSignedVarInt();

				if (response.Result != StackResponseStatus.Ok)
					continue;

				response.ResponseContainerInfos = new List<StackResponseContainerInfo>();
				var subCount = ReadUnsignedVarInt();
				for (int sub = 0; sub < subCount; sub++)
				{
					var containerInfo = new StackResponseContainerInfo();
					var name = readFullContainerName();
					containerInfo.ContainerId = name.ContainerID;
					containerInfo.DynamicId = (int)name.DynamicContainerID.Value;
					var slotCount = ReadUnsignedVarInt();
					containerInfo.Slots = new List<StackResponseSlotInfo>();

					for (int si = 0; si < slotCount; si++)
					{
						var slot = new StackResponseSlotInfo();
						slot.Slot = ReadByte();
						slot.HotbarSlot = ReadByte();
						slot.Count = ReadByte();
						slot.StackNetworkId = ReadSignedVarInt();
						slot.CustomName = ReadString();
						slot.FilteredCustomName = ReadString();
						slot.DurabilityCorrection = ReadSignedVarInt();

						containerInfo.Slots.Add(slot);
					}

					response.ResponseContainerInfos.Add(containerInfo);
				}

				responses.Add(response);
			}

			return responses;
		}
	}
}
