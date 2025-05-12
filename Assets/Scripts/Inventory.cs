using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(); // List to store the items
    public int inventorySize = 10;              // Max number of items in the inventory
    private Item currentItem;                   // The item the player is currently holding

    void Start()
    {
        // Example: Adding a weapon and a spray can to the inventory (you will add them in the editor)
        AddItem(Resources.Load<Item>("Items/Weapon"));  // Assuming you have these assets in a folder called "Items"
        AddItem(Resources.Load<Item>("Items/SprayCan"));
    }

    // Add an item to the inventory
    public bool AddItem(Item item)
    {
        if (items.Count < inventorySize)
        {
            items.Add(item);
            Debug.Log("Added item: " + item.itemName);
            return true;
        }
        else
        {
            Debug.Log("Inventory is full!");
            return false;
        }
    }

    // Remove an item from the inventory
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("Removed item: " + item.itemName);
        }
    }

    // Switch between items in the inventory (equip a new item)
    public void EquipItem(Item item)
    {
        if (items.Contains(item))
        {
            currentItem = item;
            Debug.Log("Equipped item: " + item.itemName);
        }
    }

    // Return the current item the player is holding
    public Item GetCurrentItem()
    {
        return currentItem;
    }

    // Display the inventory in the debug log
    public void DisplayInventory()
    {
        foreach (Item item in items)
        {
            Debug.Log(item.itemName + " (" + item.quantity + ")");
        }
    }
}