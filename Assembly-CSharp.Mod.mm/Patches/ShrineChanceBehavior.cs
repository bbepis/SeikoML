using System;
using MonoMod;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using SeikoML;

namespace RoR2 {

	internal class patch_ShrineChanceBehavior : ShrineChanceBehavior {
		public static event Action<bool, Interactor> onShrineChancePurchaseGlobal;

		private PurchaseInteraction purchaseInteraction;

		// Token: 0x040018D7 RID: 6359
		private int successfulPurchaseCount;

		// Token: 0x040018D8 RID: 6360
		private float refreshTimer;

		// Token: 0x040018D9 RID: 6361
		private const float refreshDuration = 2f;

		// Token: 0x040018DA RID: 6362
		private bool waitingForRefresh;

		private Xoroshiro128Plus rng;

		[MonoModPublic]
		public void AddShrineStack(Interactor activator)
		{
			if (!NetworkServer.active) {
				Debug.LogWarning("[Server] function 'System.Void RoR2.ShrineChanceBehavior::AddShrineStack(RoR2.Interactor)' called on client");
				return;
			}


			
			PickupIndex pickupIndex = ItemDropManager.GetSelection(ItemDropLocation.Shrine, this.rng.nextNormalizedFloat);
			bool flag = pickupIndex == PickupIndex.none;
			if (flag) {
				Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage {
					subjectCharacterBodyGameObject = activator.gameObject,
					baseToken = "SHRINE_CHANCE_FAIL_MESSAGE"
				});
			} else {
				this.successfulPurchaseCount++;
				PickupDropletController.CreatePickupDroplet(pickupIndex, this.dropletOrigin.position, this.dropletOrigin.forward * 20f);
				Chat.SendBroadcastChat(new Chat.SubjectFormatChatMessage {
					subjectCharacterBodyGameObject = activator.gameObject,
					baseToken = "SHRINE_CHANCE_SUCCESS_MESSAGE"
				});
			}

			Debug.Log(equipmentWeight);
			Debug.Log(failureWeight);
			Debug.Log(tier1Weight);
			Debug.Log(tier2Weight);
			Debug.Log(tier3Weight);

			Action<bool, Interactor> action = onShrineChancePurchaseGlobal;
			if (action != null) {
				action(flag, activator);
			}
			this.waitingForRefresh = true;
			this.refreshTimer = 2f;
			EffectManager.instance.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/ShrineUseEffect"), new EffectData {
				origin = base.transform.position,
				rotation = Quaternion.identity,
				scale = 1f,
				color = this.shrineColor
			}, true);
			if (this.successfulPurchaseCount >= this.maxPurchaseCount) {
				this.symbolTransform.gameObject.SetActive(false);
			}
		}
	}
}
