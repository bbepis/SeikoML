namespace SeikoML
{
    using RoR2.UI;
    using UnityEngine;

    public static class Api
    {
        // Ideal if we can get onInventoryChanged event to update this.
        public static ItemIcon[] GetItemIcons()
        {
            return GameObject.FindObjectsOfType<ItemIcon>();
        }

        public static EquipmentIcon[] GetEquipmentIcons()
        {
            return GameObject.FindObjectsOfType<EquipmentIcon>();
        }
    }
}
