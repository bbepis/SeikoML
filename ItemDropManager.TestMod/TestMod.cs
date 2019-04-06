using RoR2;
using SeikoML;
using System;
using System.Collections.Generic;

namespace FunMod {
	public class TestMod : ISeikoMod {
		public void OnStart() {
			ItemDropManager.DefaultDrops = false;
			SetupItems();
		}

		public void SetupItems() {
			var t1 = new List<ItemIndex> {
				ItemIndex.SecondarySkillMagazine,
				ItemIndex.BossDamageBonus,
				ItemIndex.BleedOnHit,
				ItemIndex.HealWhileSafe,
				ItemIndex.Mushroom,
				ItemIndex.Bear,
				ItemIndex.Hoof,
				ItemIndex.FireRing,
				ItemIndex.CritGlasses,
				ItemIndex.WardOnLevel
			};

			var t2 = new List<ItemIndex> {
				ItemIndex.Feather,
				ItemIndex.ChainLightning,
				ItemIndex.SlowOnHit,
				ItemIndex.JumpBoost,
				ItemIndex.EquipmentMagazine,
				ItemIndex.Seed
			};

			var t3 = new List<ItemIndex> {
				ItemIndex.AlienHead,
				ItemIndex.IncreaseHealing,
				ItemIndex.UtilitySkillMagazine,
				ItemIndex.ExtraLife,
				ItemIndex.FallBoots
			};

			var eq = new List<EquipmentIndex> {
				EquipmentIndex.Lightning,
				EquipmentIndex.CritOnUse,
				EquipmentIndex.Blackhole,
				EquipmentIndex.Fruit
			};

			ItemDropManager.AddDropInformation(ItemDropLocation.Chest, t1.ToSelection(0.8f), t2.ToSelection(0.2f), t3.ToSelection(0.03f));
			ItemDropManager.AddDropInformation(ItemDropLocation.Boss, t3.ToSelection());
			ItemDropManager.AddDropInformation(ItemDropLocation.Shrine, ItemDropManager.None.ToSelection(0.5f), t1.ToSelection(0.8f), t2.ToSelection(0.2f), t3.ToSelection(0.03f));
		}
	}
}
