using System;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public static class PacketFactory
	{
		
		public static Packet translatePacket(int id, ReadOnlyMemory<byte> buffer)
		{
				switch (id)
				{
					case 1:
						return new McbeLogin().Decode(buffer);
					case 2:
						return new McbePlayStatus().Decode(buffer);
					case 3:
						return new McbeServerToClientHandshake().Decode(buffer);
					case 4:
						return new McbeClientToServerHandshake().Decode(buffer);
					case 5:
						return new McbeDisconnect().Decode(buffer);
					case 6:
						return new McbeResourcePacksInfo().Decode(buffer);
					case 7:
						return new McbeResourcePackStack().Decode(buffer);
					case 8:
						return new McbeResourcePackClientResponse().Decode(buffer);
					case 9:
						return new McbeText().Decode(buffer);
					case 10:
						return new McbeSetTime().Decode(buffer);
					case 11:
						return new McbeStartGame().Decode(buffer);
					case 12:
						return new McbeAddPlayer().Decode(buffer);
					case 13:
						return new McbeAddEntity().Decode(buffer);
					case 14:
						return new McbeRemoveEntity().Decode(buffer);
					case 15:
						return new McbeAddItemEntity().Decode(buffer);
					case 16:
						return new McbeServerPlayerPostMovePosition().Decode(buffer);

					case 17:
						return new McbeTakeItemEntity().Decode(buffer);
					case 18:
						return new McbeMoveEntity().Decode(buffer);
					case 19:
						return new McbeMovePlayer().Decode(buffer);

					case 21:
						return new McbeUpdateBlock().Decode(buffer);
					case 22:
						return new McbeAddPainting().Decode(buffer);

					case 23:
						return new McbeTickSync().Decode(buffer);

					case 25:
						return new McbeLevelEvent().Decode(buffer);
					case 26:
						return new McbeBlockEvent().Decode(buffer);
					case 27:
						return new McbeEntityEvent().Decode(buffer);
					case 28:
						return new McbeMobEffect().Decode(buffer);
					case 29:
						return new McbeUpdateAttributes().Decode(buffer);
					case 30:
						return new McbeInventoryTransaction().Decode(buffer);
					case 31:
						return new McbeMobEquipment().Decode(buffer);
					case 32:
						return new McbeMobArmorEquipment().Decode(buffer);
					case 33:
						return new McbeInteract().Decode(buffer);
					case 34:
						return new McbeBlockPickRequest().Decode(buffer);
					case 35:
						return new McbeEntityPickRequest().Decode(buffer);
					case 36:
						return new McbePlayerAction().Decode(buffer);

					case 38:
						return new McbeHurtArmor().Decode(buffer);
					case 39:
						return new McbeSetEntityData().Decode(buffer);
					case 40:
						return new McbeSetEntityMotion().Decode(buffer);
					case 41:
						return new McbeSetEntityLink().Decode(buffer);
					case 42:
						return new McbeSetHealth().Decode(buffer);
					case 43:
						return new McbeSetSpawnPosition().Decode(buffer);
					case 44:
						return new McbeAnimate().Decode(buffer);
					case 45:
						return new McbeRespawn().Decode(buffer);
					case 46:
						return new McbeContainerOpen().Decode(buffer);
					case 47:
						return new McbeContainerClose().Decode(buffer);
					case 48:
						return new McbePlayerHotbar().Decode(buffer);
					case 49:
						return new McbeInventoryContent().Decode(buffer);
					case 50:
						return new McbeInventorySlot().Decode(buffer);
					case 51:
						return new McbeContainerSetData().Decode(buffer);
					case 52:
						return new McbeCraftingData().Decode(buffer);

					case 54:
						return new McbeGuiDataPickItem().Decode(buffer);
					case 55:
						return new McbeAdventureSettings().Decode(buffer);
					case 56:
						return new McbeBlockEntityData().Decode(buffer);

					case 58:
						return new McbeLevelChunk().Decode(buffer);
					case 59:
						return new McbeSetCommandsEnabled().Decode(buffer);
					case 60:
						return new McbeSetDifficulty().Decode(buffer);
					case 61:
						return new McbeChangeDimension().Decode(buffer);
					case 62:
						return new McbeSetPlayerGameType().Decode(buffer);
					case 63:
						return new McbePlayerList().Decode(buffer);
					case 64:
						return new McbeSimpleEvent().Decode(buffer);
					case 65:
						return new McbeTelemetryEvent().Decode(buffer);
					case 66:
						return new McbeSpawnExperienceOrb().Decode(buffer);
					case 67:
						return new McbeClientboundMapItemData().Decode(buffer);
					case 68:
						return new McbeMapInfoRequest().Decode(buffer);
					case 69:
						return new McbeRequestChunkRadius().Decode(buffer);
					case 70:
						return new McbeChunkRadiusUpdate().Decode(buffer);

					case 72:
						return new McbeGameRulesChanged().Decode(buffer);
					case 73:
						return new McbeCamera().Decode(buffer);
					case 74:
						return new McbeBossEvent().Decode(buffer);
					case 75:
						return new McbeShowCredits().Decode(buffer);
					case 76:
						return new McbeAvailableCommands().Decode(buffer);
					case 77:
						return new McbeCommandRequest().Decode(buffer);
					case 78:
						return new McbeCommandBlockUpdate().Decode(buffer);
					case 79:
						return new McbeCommandOutput().Decode(buffer);
					case 80:
						return new McbeUpdateTrade().Decode(buffer);
					case 81:
						return new McbeUpdateEquipment().Decode(buffer);
					case 82:
						return new McbeResourcePackDataInfo().Decode(buffer);
					case 83:
						return new McbeResourcePackChunkData().Decode(buffer);
					case 84:
						return new McbeResourcePackChunkRequest().Decode(buffer);
					case 85:
						return new McbeTransfer().Decode(buffer);
					case 86:
						return new McbePlaySound().Decode(buffer);
					case 87:
						return new McbeStopSound().Decode(buffer);
					case 88:
						return new McbeSetTitle().Decode(buffer);
					case 89:
						return new McbeAddBehaviorTree().Decode(buffer);
					case 90:
						return new McbeStructureBlockUpdate().Decode(buffer);
					case 91:
						return new McbeShowStoreOffer().Decode(buffer);
					case 92:
						return new McbePurchaseReceipt().Decode(buffer);
					case 93:
						return new McbePlayerSkin().Decode(buffer);
					case 94:
						return new McbeSubClientLogin().Decode(buffer);
					case 95:
						return new McbeInitiateWebSocketConnection().Decode(buffer);
					case 96:
						return new McbeSetLastHurtBy().Decode(buffer);
					case 97:
						return new McbeBookEdit().Decode(buffer);
					case 98:
						return new McbeNpcRequest().Decode(buffer);
					case 99:
						return new McbePhotoTransfer().Decode(buffer);
					case 100:
						return new McbeModalFormRequest().Decode(buffer);
					case 101:
						return new McbeModalFormResponse().Decode(buffer);
					case 102:
						return new McbeServerSettingsRequest().Decode(buffer);
					case 103:
						return new McbeServerSettingsResponse().Decode(buffer);
					case 104:
						return new McbeShowProfile().Decode(buffer);
					case 105:
						return new McbeSetDefaultGameType().Decode(buffer);
					case 106:
						return new McbeRemoveObjective().Decode(buffer);
					case 107:
						return new McbeSetDisplayObjective().Decode(buffer);
					case 108:
						return new McbeSetScore().Decode(buffer);
					case 109:
						return new McbeLabTable().Decode(buffer);
					case 110:
						return new McbeUpdateBlockSynced().Decode(buffer);
					case 111:
						return new McbeMoveEntityDelta().Decode(buffer);
					case 112:
						return new McbeSetScoreboardIdentity().Decode(buffer);
					case 113:
						return new McbeSetLocalPlayerAsInitialized().Decode(buffer);
					case 114:
						return new McbeUpdateSoftEnum().Decode(buffer);
					case 115:
						return new McbeNetworkStackLatency().Decode(buffer);

					case 117:
						return new McbeScriptCustomEvent().Decode(buffer);
					case 118:
						return new McbeSpawnParticleEffect().Decode(buffer);
					case 119:
						return new McbeAvailableEntityIdentifiers().Decode(buffer);

					case 121:
						return new McbeNetworkChunkPublisherUpdate().Decode(buffer);
					case 122:
						return new McbeBiomeDefinitionList().Decode(buffer);
					case 123:
						return new McbeLevelSoundEvent().Decode(buffer);
					case 124:
						return new McbeLevelEventGeneric().Decode(buffer);
					case 125:
						return new McbeLecternUpdate().Decode(buffer);


					case 129:
						return new McbeClientCacheStatus().Decode(buffer);
					case 130:
						return new McbeOnScreenTextureAnimation().Decode(buffer);
					case 131:
						return new McbeMapCreateLockedCopy().Decode(buffer);
					case 132:
						return new McbeStructureTemplateDataExportRequest().Decode(buffer);
					case 133:
						return new McbeStructureTemplateDataExportResponse().Decode(buffer);

					case 135:
						return new McbeClientCacheBlobStatus().Decode(buffer);
					case 136:
						return new McbeClientCacheMissResponse().Decode(buffer);
					case 137:
						return new McbeEducationSettings().Decode(buffer);
					case 138:
						return new McbeEmotePacket().Decode(buffer);
					case 139:
						return new McbeMultiPlayerSettings().Decode(buffer);
					case 140:
						return new McbeSettingsCommand().Decode(buffer);
					case 141:
						return new McbeAnvilDamage().Decode(buffer);
					case 142:
						return new McbeCompletedUsingItem().Decode(buffer);
					case 143:
						return new McbeNetworkSettings().Decode(buffer);
					case 144:
						return new McbePlayerAuthInput().Decode(buffer);
					case 145:
						return new McbeCreativeContent().Decode(buffer);
					case 146:
						return new McbePlayerEnchantOptions().Decode(buffer);
					case 147:
						return new McbeItemStackRequest().Decode(buffer);
					case 148:
						return new McbeItemStackResponse().Decode(buffer);
					case 149:
						return new McbeHurtArmor().Decode(buffer);
					case 150:
						return new McbeCodeBuilder().Decode(buffer);
					case 151:
						return new McbeUpdatePlayerGameType().Decode(buffer);
					case 152:
						return new McbeEmoteList().Decode(buffer);
					case 153:
						return new McbePositionTrackingDBServerBroadcast().Decode(buffer);
					case 154:
						return new McbePositionTrackingDBClientRequest().Decode(buffer);
					case 155:
						return new McbeDebugInfo().Decode(buffer);

					case 156:
						return new McbePacketViolationWarning().Decode(buffer);
					case 157:
						return new McbeMotionPredictionHints().Decode(buffer);

					case 158:
						return new McbeAnimateEntity().Decode(buffer);

					case 159:
						return new McbeCamera().Decode(buffer);

					case 160:
						return new McbePlayerFog().Decode(buffer);
					case 161:
						return new McbeCorrectPlayerMovePrediction().Decode(buffer);

					case 162:
						return new McbeItemRegistry().Decode(buffer);

					case 163:
						return new McbeFilterTextPacket().Decode(buffer);
					case 164:
						return new McbeClientBoundDebugRenderer().Decode(buffer);
					case 165:
						return new McbeSyncEntityProperty().Decode(buffer);
					case 166:
						return new McbeAddVolumeEntity().Decode(buffer);

					case 167:
						return new McbeRemoveVolumeEntity().Decode(buffer);

					case 168:
						return new McbeSimulationType().Decode(buffer);

					case 169:
						return new McbeNPCDialogue().Decode(buffer);

					case 170:
						return new McbeEducationResourceURI().Decode(buffer);

					case 171:
						return new McbeCreatePhoto().Decode(buffer);

					case 172:
						return new McbeUpdateSubChunkBlocksPacket().Decode(buffer);
					case 173:
						return new McbePhotoInfoRequest().Decode(buffer);

					case 174:
						return new McbeSubChunkPacket().Decode(buffer);
					case 175:
						return new McbeSubChunkRequestPacket().Decode(buffer);
					case 176:
						return new McbeClientStartItemCooldown().Decode(buffer);

					case 177:
						return new McbeScriptMessage().Decode(buffer);

					case 178:
						return new McbeCodeBuilderSource().Decode(buffer);

					case 179:
						return new McbeTickingAreasLoadStatus().Decode(buffer);

					case 180:
						return new McbeDimensionData().Decode(buffer);
					case 181:
						return new McbeAgentAction().Decode(buffer);

					case 182:
						return new McbeChangeMobProperty().Decode(buffer);

					case 183:
						return new McbeLessonProgress().Decode(buffer);

					case 184:
						return new McbeRequestAbility().Decode(buffer);
					case 185:
						return new McbePermissionRequest().Decode(buffer);
					case 186:

					case 187:
						return new McbeUpdateAbilities().Decode(buffer);
					case 188:
						return new McbeUpdateAdventureSettings().Decode(buffer);
					case 189:
						return new McbeDeathInfo().Decode(buffer);

					case 190:
						return new McbeEditorNetwork().Decode(buffer);

					case 191:
						return new McbeFeatureRegistry().Decode(buffer);

					case 192:
						return new McbeServerStats().Decode(buffer);

					case 193:
						return new McbeRequestNetworkSettings().Decode(buffer);
					case 194:
						return new McbeGameTestRequest().Decode(buffer);

					case 195:
						return new McbeGameTestResults().Decode(buffer);

					case 196:
						return new McbeUpdateClientInputLocks().Decode(buffer);

					case 197:
						return new McbeClientCheatAbility().Decode(buffer);

					case 198:
						return new McbeCameraPresets().Decode(buffer);

					case 199:
						return new McbeUnlockedRecipes().Decode(buffer);

					case 300:
						return new McbeCameraInstruction().Decode(buffer);


					case 302:
						return new McbeTrimData().Decode(buffer);
					case 303:
						return new McbeOpenSign().Decode(buffer);
					case 304:
						return new McbeAlexEntityAnimation().Decode(buffer);
					case 305:
						return new McbeRefreshEntitlements().Decode(buffer);

					case 306:
						return new McbePlayerToggleCrafterSlotRequest().Decode(buffer);

					case 307:
						return new McbeSetInventoryOptions().Decode(buffer);
					case 308:
						return new McbeSetHud().Decode(buffer);

					case 309:
						return new McbeAwardAchievement().Decode(buffer);

					case 310:
						return new McbeClientBoundCloseForm().Decode(buffer);


					case 312:
						return new McbeServerboundLoadingScreen().Decode(buffer);
					case 313:
						return new McbeJigsawStructureData().Decode(buffer);

					case 314:
						return new McbeCurrentStructureFeature().Decode(buffer);

					case 315:
						return new McbeServerBoundDiagnostics().Decode(buffer);

					case 316:
						return new McbeCameraAimAssist().Decode(buffer);

					case 317:
						return new McbeContainerRegistryCleanup().Decode(buffer);

					case 318:
						return new McbeMovementEffect().Decode(buffer);


					case 320:
						return new McbeCameraAimAssistPresets().Decode(buffer);

					case 321:
						return new McbeClientCameraAimAssist().Decode(buffer);

					case 322:
						return new McbeClientMovementPredictionSync().Decode(buffer);

					case 323:
						return new McbeUpdateClientOptions().Decode(buffer);

					case 324:
						return new McbePlayerVideoCapture().Decode(buffer);

					case 325:
						return new McbePlayerUpdateEntityOverrides().Decode(buffer);

					case 326:
						return new McbePlayerLocation().Decode(buffer);

					case 327:
						return new McbeClientBoundControlSchemeSet().Decode(buffer);
					case 328:
						return new McbeDebugDrawer().Decode(buffer);
					case 329:
						return new McbeServerBoundPackSettingChange().Decode(buffer);
					case 330:
						return new McbeClientboundDataStore().Decode(buffer);
					case 331:
						return new McbeGraphicsOverrideParameter().Decode(buffer);
					case 332:
						return new McbeServerboundDataStore().Decode(buffer);
					case 333:
						return new McbeClientBoundDataDrivenUIShowScreen().Decode(buffer);
					case 334:
						return new McbeClientBoundDataDrivenUICloseAllScreens().Decode(buffer);
					case 335:
						return new McbeClientBoundDataDrivenUIReload().Decode(buffer);
					case 336:
						return new McbeClientBoundTextureShift().Decode(buffer);
					case 337:
						return new McbeVoxelShapes().Decode(buffer);
					case 338:
						return new McbeCameraSpline().Decode(buffer);
					case 339:
						return new McbeCameraAimAssistActorPriority().Decode(buffer);
					case 340:
						return new McbeResourcePacksReadyForValidation().Decode(buffer);
					case 341:
						return new McbeLocatorBar().Decode(buffer);
					case 342:
						return new McbePartyChanged().Decode(buffer);
					case 343:
						return new McbeServerBoundDataDrivenScreenClosed().Decode(buffer);
					case 344:
						return new McbeSyncWorldClocks().Decode(buffer);
					case 345:
						return new McbeClientBoundAttributeLayerSync().Decode(buffer);
					case 346:
						return new McbeServerStoreInfo().Decode(buffer);
					case 347:
						return new McbeServerPresenceInfo().Decode(buffer);
					default:
						return new UnknownPacket((byte)id, buffer);
				}
			}
		}
	}
