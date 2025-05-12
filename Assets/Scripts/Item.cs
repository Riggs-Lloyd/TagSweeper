using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;      // Name of the item (e.g., "Spray Can", "Weapon")
    public Sprite itemIcon;      // Item icon (for UI purposes)
    public ItemType itemType;    // Type of the item (e.g., Weapon, Tool)
    public int quantity;         // Quantity for stackable items (e.g., bullets or spray)

    public enum ItemType
    {
        Weapon,
        Tool,
        Consumable,
        Misc
    }

    // Constructor is not needed for ScriptableObject-based items
}
