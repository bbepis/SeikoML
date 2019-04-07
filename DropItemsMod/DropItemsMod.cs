namespace DropItemsMod
{
    using SeikoML;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using RoR2;
    using RoR2.UI;

    public class DropItemHandler : MonoBehaviour, IPointerClickHandler
    {
        public EquipmentIndex EquipmentIndex { get; set; } = EquipmentIndex.None;
        public ItemIndex ItemIndex { get; set; } = ItemIndex.None;
        public Inventory Inventory { get; set; } = null;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (Inventory == null) return;
            Transform transform = Inventory.GetComponent<CharacterMaster>().GetBody().transform;
            
            if (EquipmentIndex != EquipmentIndex.None)
            {
                Inventory.SetEquipmentIndex(EquipmentIndex.None);
                Resources.Load<SpawnCard>("SpawnCards/InteractableSpawnCard/iscLockbox").DoSpawn(transform.position, Quaternion.identity).GetComponent<ChestBehavior>().SetDropPickup(EquipmentIndex);
                return;
            }

            Inventory.RemoveItem(ItemIndex, 1);
            Resources.Load<SpawnCard>("SpawnCards/InteractableSpawnCard/iscLockbox").DoSpawn(transform.position, Quaternion.identity).GetComponent<ChestBehavior>().SetDropPickup(ItemIndex);
        }
    }

    public class DropItemsMod : IKookehsMod
    {
        public void Start()
        {
            Debug.Log("Loaded DropItemsMod");
        }

        public void Update()
        {
            // TODO(kookehs): Check if a run has started
            ItemIcon[] itemIcons = Api.GetItemIcons();

            foreach (ItemIcon itemIcon in itemIcons)
            {
                if (itemIcon.GetComponent<DropItemHandler>() == null)
                {
                    DropItemHandler dropItemHandler = itemIcon.transform.gameObject.AddComponent<DropItemHandler>();
                    dropItemHandler.ItemIndex = itemIcon.ItemIndex;
                    dropItemHandler.Inventory = itemIcon.rectTransform.parent.GetComponent<ItemInventoryDisplay>().Inventory;
                }
            }

            EquipmentIcon[] equipmentIcons = Api.GetEquipmentIcons();

            foreach (EquipmentIcon equipmentIcon in equipmentIcons)
            {
                if (equipmentIcon.GetComponent<DropItemHandler>() == null)
                {
                    if (equipmentIcon.targetEquipmentSlot == null || equipmentIcon.targetEquipmentSlot.equipmentIndex == EquipmentIndex.None) return;
                    DropItemHandler dropItemHandler = equipmentIcon.transform.gameObject.AddComponent<DropItemHandler>();
                    dropItemHandler.EquipmentIndex = equipmentIcon.targetEquipmentSlot.equipmentIndex;
                }
            }
        }
    }
}
