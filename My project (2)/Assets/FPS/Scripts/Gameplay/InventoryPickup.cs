using UnityEngine;

namespace Unity.FPS.Gameplay
{
    public class InventoryPickup : Pickup
    {
        public InventoryItem Item;
        [Min(1)] public int Amount = 1;

        protected override void OnPicked(PlayerCharacterController playerController)
        {
            PlayerInventory inventory = playerController.GetComponent<PlayerInventory>();
            if (inventory == null || !inventory.AddItem(Item, Amount))
                return;

            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}