using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using SeikoML;

namespace RoR2 {
	internal class patch_BossGroup : BossGroup
	{
		[MonoModIgnore]
		private extern void RemoveMemberAt(int memberIndex);

		// Token: 0x04000F9D RID: 3997
		private Xoroshiro128Plus rng;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000BB6 RID: 2998 RVA: 0x000395A4 File Offset: 0x000377A4
		// (remove) Token: 0x06000BB7 RID: 2999 RVA: 0x000395D8 File Offset: 0x000377D8
		[MonoModIgnore]
		public static event Action<BossGroup> onBossGroupDefeatedServer;

		// Token: 0x04000F9C RID: 3996
		private bool defeated;

		// Token: 0x04000FA2 RID: 4002
		private List<PickupIndex> bossDrops;

		// Token: 0x04000F99 RID: 3993
		private readonly List<CharacterMaster> membersList;

		private void OnCharacterDeathCallback(DamageReport damageReport) {
			if (!NetworkServer.active) {
				Debug.LogWarning("[Server] function 'System.Void RoR2.BossGroup::OnCharacterDeathCallback(RoR2.DamageReport)' called on client");
				return;
			}
			DamageInfo damageInfo = damageReport.damageInfo;
			GameObject gameObject = damageReport.victim.gameObject;
			CharacterBody component = gameObject.GetComponent<CharacterBody>();
			if (!component) {
				return;
			}
			CharacterMaster master = component.master;
			if (!master) {
				return;
			}
			DeathRewards component2 = gameObject.GetComponent<DeathRewards>();
			if (component2) {
				PickupIndex pickupIndex = (PickupIndex)component2.bossPickup;
				if (pickupIndex != PickupIndex.none && ItemDropManager.IncludeSpecialBossDrops) {
					this.bossDrops.Add(pickupIndex);
				}
			}
			GameObject victimMasterGameObject = master.gameObject;
			int num = this.membersList.FindIndex((CharacterMaster x) => x.gameObject == victimMasterGameObject);
			if (num >= 0) {
				this.RemoveMemberAt(num);
				if (!this.defeated && this.membersList.Count == 0) {
					Run.instance.OnServerBossKilled(true);
					if (component) {
						int participatingPlayerCount = Run.instance.participatingPlayerCount;
						if (participatingPlayerCount != 0 && this.dropPosition) {
                            //ItemIndex itemIndex = ItemDropManager.BossDropList[this.rng.RangeInt(0, ItemDropManager.BossDropList.Count)];
                            ItemIndex itemIndex = ItemDropManager.GetSelection(ItemDropLocation.Boss, rng.nextNormalizedFloat).itemIndex;
							int num2 = participatingPlayerCount * (1 + (TeleporterInteraction.instance ? TeleporterInteraction.instance.shrineBonusStacks : 0));
							float angle = 360f / (float)num2;
							Vector3 vector = Quaternion.AngleAxis((float)UnityEngine.Random.Range(0, 360), Vector3.up) * (Vector3.up * 40f + Vector3.forward * 5f);
							Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
							int i = 0;
							while (i < num2) {
								PickupIndex pickupIndex2 = new PickupIndex(itemIndex);
								if (this.bossDrops.Count > 0 && this.rng.nextNormalizedFloat <= this.bossDropChance) {
									pickupIndex2 = this.bossDrops[this.rng.RangeInt(0, this.bossDrops.Count)];
								}
								PickupDropletController.CreatePickupDroplet(pickupIndex2, this.dropPosition.position, vector);
								i++;
								vector = rotation * vector;
							}
						}
					}
					this.defeated = true;
					Action<BossGroup> action = onBossGroupDefeatedServer;
					if (action == null) {
						return;
					}
					action(this);
					return;
				} else {
					Run.instance.OnServerBossKilled(false);
				}
			}
		}

	}
}
