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
            var foundItem = items.FirstOrDefault(i => i.GetInteractableType() == item.GetType());

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

    public IEnumerable<InventoryItem> FindItems<T>()
    {
        var foundItems = items.Where(i => i.IsType<T>());
        return foundItems;
    }
}
