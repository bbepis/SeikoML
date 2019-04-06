using System.Collections.Generic;
using System.Linq;

namespace RoR2 {
    public class PickupSelection
    {
        public List<PickupIndex> Pickups { get; set; }
        public float DropChance { get; set; } = 1.0f;
    }

	public static class DefaultItemDrops
	{
		public static void AddDefaults()
		{
			if (ItemDropManager.DefaultDrops) {
				AddDefaultShrineDrops();
			}
		}

		private static void AddDefaultShrineDrops() {
			PickupIndex none = PickupIndex.none;
			var shrineSelections = new List<PickupSelection> {
				ItemDropManager.None.ToSelection(ItemDropManager.DefaultShrineFailureWeight),
				ItemDropManager.Tier1DropList.ToSelection(ItemDropManager.DefaultShrineTier1Weight),
				ItemDropManager.Tier2DropList.ToSelection(ItemDropManager.DefaultShrineTier2Weight),
				ItemDropManager.Tier3DropList.ToSelection(ItemDropManager.DefaultShrineTier3Weight),
				ItemDropManager.EquipmentList.ToSelection(ItemDropManager.DefaultShrineEquipmentWeight)
			};

			ItemDropManager.AddDropInformation(ItemDropLocation.Shrine, shrineSelections);
		}
	}

	public enum ItemDropLocation {
		Boss,
		Chest,
		Shrine
	}

	public static class ItemDropManager
	{
		public static bool DefaultDrops { get; set; } = true;

		public static List<ItemIndex> None { get; set; } = new List<ItemIndex> { ItemIndex.None };


		public static void AddDropInformation(ItemDropLocation dropLocation, params PickupSelection[] pickupSelections)
        {
            Selection[dropLocation] = pickupSelections.ToList();
        }

		public static void AddDropInformation(ItemDropLocation dropLocation, List<PickupSelection> pickupSelections) {
			Selection[dropLocation] = pickupSelections;
		}

		public static PickupIndex GetSelection(ItemDropLocation dropLocation, float normalizedIndex)
        {
            var selections = Selection[dropLocation];
            var weightedSelection = new WeightedSelection<PickupIndex>();
            foreach (var selection in selections)
                foreach (var pickup in selection.Pickups)
                    weightedSelection.AddChoice(pickup, selection.DropChance / selection.Pickups.Count);

            return weightedSelection.Evaluate(normalizedIndex);
        }


		public static List<ItemIndex> Tier1DropList { get; set; }

		public static List<ItemIndex> Tier2DropList { get; set; }

		public static List<ItemIndex> Tier3DropList { get; set; }

		public static List<EquipmentIndex> EquipmentList { get; set; }

		public static List<ItemIndex> LunarDropList { get; set; }

		public static Dictionary<ItemDropLocation, List<PickupSelection>> Selection { get; set; }

		public static PickupSelection ToSelection(this List<ItemIndex> indices, float dropChance = 1.0f) {
			return new PickupSelection {
				DropChance = dropChance,
				Pickups = indices.Select(x => new PickupIndex(x)).ToList()
			};
		}

		public static PickupSelection ToSelection(this List<EquipmentIndex> indices, float dropChance = 1.0f) {
			return new PickupSelection {
				DropChance = dropChance,
				Pickups = indices.Select(x => new PickupIndex(x)).ToList()
			};
		}


		public static bool IncludeSpecialBossDrops = true;

		public static float DefaultChestTier1DropChance = 0.8f;
		public static float DefaultChestTier2DropChance = 0.2f;
		public static float DefaultChestTier3DropChance = 0.01f;

		public static float DefaultShrineEquipmentWeight = 2f;
		public static float DefaultShrineFailureWeight = 10.1f;
		public static float DefaultShrineTier1Weight = 8f;
		public static float DefaultShrineTier2Weight = 2f;
		public static float DefaultShrineTier3Weight = 0.2f;
	}
}
