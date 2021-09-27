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
                foundItem.IncreaseQuantity(item.GetQuantity());
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

            RemoveItem(inventoryItem.GetId());
        }
    }

    public bool TryConsumeItem(int id,int quantity, out int notConsumed,bool forceConsume = false)
    {
        notConsumed = quantity;

        var foundItem = FindItem(id);

        if (foundItem != null)
        {
            int totalQuantity = foundItem.quantity;
            int delta = totalQuantity - quantity;
            if (delta >= 0)
            {
                foundItem.DecreaseQuantity(quantity);
                notConsumed = 0;
                return true;
            }
            else if(forceConsume)
            {
                foundItem.DecreaseQuantity(totalQuantity);
                notConsumed = Math.Abs(delta); 
                return true;
            }
        }

        return false;
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
            quantity = item.GetQuantity()
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
