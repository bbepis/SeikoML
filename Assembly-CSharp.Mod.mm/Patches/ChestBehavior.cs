using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityStates;
using EntityStates.Barrel;
using MonoMod;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace RoR2 {
	internal class patch_ChestBehavior : ChestBehavior
	{
		[MonoModIgnore]
		private extern void PickFromList(List<PickupIndex> dropList);

		private PickupIndex dropPickup;

		[MonoModPublic]
		public void RollItem() {
			if (!NetworkServer.active) {
				Debug.LogWarning("[Server] function 'System.Void RoR2.ChestBehavior::RollItem()' called on client");
				return;
			}

			this.dropPickup = ItemDropManager.GetSelection(ItemDropLocation.Chest,
				Run.instance.treasureRng.nextNormalizedFloat);
		}
	}
}
