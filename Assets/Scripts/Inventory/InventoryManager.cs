using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(IInteractable item)
    {
        if (item.IsStackable())
        {
            var foundItem = FindItem(item.GetId());

            if (foundItem != null)
            {
                foundItem.IncreaseQuantity();
                return;
            }
        }

        var inventoryItem = AddNewItem(item);
        inventoryItem.onQuantityChanged += OnInventoryItemQuantityChanged;
    }

    private void OnInventoryItemQuantityChanged(object sender, int quantity)
    {
        if (quantity <= 0)
        {
            var inventoryItem = (InventoryItem)sender;


        }
    }

    public bool RemoveItem(int id)
    {
        var foundItem = FindItem(id);

        if (foundItem != null)
        {
            return items.Remove(foundItem);
        }

        return false;
    }

    public void RemoveAllItem(int id)
    {
        var value = items.RemoveAll(i => i.GetId() == id);
    }

    private InventoryItem AddNewItem(IInteractable item)
    {
        var inventoryItem = new InventoryItem()
        {
            interactable = item,
            quantity = 1
        };

        items.Add(inventoryItem);

        return inventoryItem;
    }

    public int GetItemsCount()
    {
        return items.Count;
    }

    public InventoryItem FindItem(int id)
    {
        return items.FirstOrDefault(i => i.GetId() == id);
    }

    public IEnumerable<InventoryItem> FindItems(int id)
    {
        var foundItems = items.Where(i => i.GetId() == id);
        return foundItems;
    }
}
