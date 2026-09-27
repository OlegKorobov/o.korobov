using Unity.FPS.Game;
using UnityEngine;

namespace Unity.FPS.Gameplay
{
    [CreateAssetMenu(menuName = "Inventory/Item")]
    public class InventoryItem : ScriptableObject
    {
        public string DisplayName = "Item";
        public Sprite Icon;
        [Min(1)] public int MaxStack = 10;
        [Min(0f)] public float HealAmount;

        public virtual bool Use(PlayerInventory inventory)
        {
            if (HealAmount <= 0f)
                return false;

            Health health = inventory.GetComponent<Health>();
            if (health == null || !health.CanPickup())
                return false;

            health.Heal(HealAmount);
            return true;
        }
    }
}