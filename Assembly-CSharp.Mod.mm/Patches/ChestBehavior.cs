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
using SeikoML;

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

			if (dropPickup != PickupIndex.none)
			{
				return;
			}

			if (tier1Chance == 0.8f)
			{
				this.dropPickup = ItemDropManager.GetSelection(ItemDropLocation.SmallChest,
					Run.instance.treasureRng.nextNormalizedFloat);
			}
			else if (tier2Chance == 0.8f)
			{
				this.dropPickup = ItemDropManager.GetSelection(ItemDropLocation.MediumChest,
					Run.instance.treasureRng.nextNormalizedFloat);
			}
			else
			{
				this.dropPickup = ItemDropManager.GetSelection(ItemDropLocation.LargeChest,
					Run.instance.treasureRng.nextNormalizedFloat);
			}
		}

		[MonoModPublic]
		public void RollEquipment() {
			if (!NetworkServer.active) {
				Debug.LogWarning("[Server] function 'System.Void RoR2.ChestBehavior::RollEquipment()' called on client");
				return;
			}
			
			this.dropPickup = ItemDropManager.GetSelection(ItemDropLocation.EquipmentChest,
				Run.instance.treasureRng.nextNormalizedFloat);
		}

		public void SetDropPickup(EquipmentIndex equipmentIndex)
		{
			dropPickup = new PickupIndex(equipmentIndex);
		}

		public void SetDropPickup(ItemIndex itemIndex)
		{
			dropPickup = new PickupIndex(itemIndex);
		}
	}
}
