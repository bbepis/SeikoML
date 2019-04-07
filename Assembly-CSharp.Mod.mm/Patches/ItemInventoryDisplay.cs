namespace RoR2.UI
{
    class patch_ItemInventoryDisplay : ItemInventoryDisplay
    {
        // Redefine access level
        public Inventory Inventory
        {
            get
            {
                return inventory;
            }
        }
    }
}
