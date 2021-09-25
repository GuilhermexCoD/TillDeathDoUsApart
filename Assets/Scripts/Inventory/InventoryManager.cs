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
                foundItem.IncreaseQuantity(1);
                return;
            }
        }

        AddNewItem(item);
    }

    private void AddNewItem(IInteractable item)
    {
        var intaractable = new InventoryItem()
        {
            interactable = item,
            quantity = 1
        };

        items.Add(intaractable);
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
