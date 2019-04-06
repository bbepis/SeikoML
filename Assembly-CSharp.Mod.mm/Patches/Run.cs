using MonoMod;
using System.Collections.Generic;
using System.Linq;
using RoR2;

namespace RoR2
{
    internal class patch_Run : Run
    {

        [MonoModPublic]
        public void BuildDropTable()
        {
			// These no longer do anything afaik
	        this.availableTier1DropList.Clear();
	        this.availableTier2DropList.Clear();
	        this.availableTier3DropList.Clear();
	        this.availableLunarDropList.Clear();
	        this.availableEquipmentDropList.Clear();

	        if (!ItemDropManager.DefaultDrops)
	        {
		        return;
	        }

	        for (ItemIndex itemIndex = ItemIndex.Syringe; itemIndex < ItemIndex.Count; itemIndex++) {
		        if (this.availableItems.HasItem(itemIndex)) {
			        ItemDef itemDef = ItemCatalog.GetItemDef(itemIndex);
			        List<PickupIndex> list = null;
			        switch (itemDef.tier) {
				        case ItemTier.Tier1:
					        list = this.availableTier1DropList;
					        break;
				        case ItemTier.Tier2:
					        list = this.availableTier2DropList;
					        break;
				        case ItemTier.Tier3:
					        list = this.availableTier3DropList;
					        break;
				        case ItemTier.Lunar:
					        list = this.availableLunarDropList;
					        break;
			        }
			        if (list != null) {
				        list.Add(new PickupIndex(itemIndex));
			        }
		        }
	        }

	        for (EquipmentIndex equipmentIndex = EquipmentIndex.CommandMissile; equipmentIndex < EquipmentIndex.Count; equipmentIndex++) {
		        if (this.availableEquipment.HasEquipment(equipmentIndex)) {
			        EquipmentDef equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
			        if (equipmentDef.canDrop) {
				        if (!equipmentDef.isLunar) {
					        this.availableEquipmentDropList.Add(new PickupIndex(equipmentIndex));
				        } else {
					        this.availableLunarDropList.Add(new PickupIndex(equipmentIndex));
				        }
			        }
		        }
	        }

	        ItemDropManager.Tier1DropList = availableTier1DropList.Select(x => x.itemIndex).ToList();
			ItemDropManager.Tier2DropList = availableTier2DropList.Select(x => x.itemIndex).ToList();
			ItemDropManager.Tier3DropList = availableTier3DropList.Select(x => x.itemIndex).ToList();
			ItemDropManager.EquipmentList = availableEquipmentDropList.Select(x => x.equipmentIndex).ToList();
			ItemDropManager.LunarDropList = availableLunarDropList.Select(x => x.itemIndex).ToList();

	        this.smallChestDropTierSelector.Clear();
	        this.smallChestDropTierSelector.AddChoice(this.availableTier1DropList, 0.8f);
	        this.smallChestDropTierSelector.AddChoice(this.availableTier2DropList, 0.2f);
	        this.smallChestDropTierSelector.AddChoice(this.availableTier3DropList, 0.01f);
	        this.mediumChestDropTierSelector.Clear();
	        this.mediumChestDropTierSelector.AddChoice(this.availableTier2DropList, 0.8f);
	        this.mediumChestDropTierSelector.AddChoice(this.availableTier3DropList, 0.2f);
	        this.largeChestDropTierSelector.Clear();
				
		}
    }
}
