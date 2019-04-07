namespace RoR2
{
    using UnityEngine.Networking;

    class patch_ChestBehavior : ChestBehavior
    {
        private extern void orig_RollItem();

        public void RollItem()
        {
            if (dropPickup != PickupIndex.none)
            {
                return;
            }
            orig_RollItem();
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
