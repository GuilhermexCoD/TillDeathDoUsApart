using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public IInteractable interactable { get; set; }
    public int quantity { get; set; }

    public event EventHandler<int> onQuantityChanged;

    public int GetId()
    {
        return interactable.GetId();
    }

    public bool IsStackable()
    {
        return interactable.IsStackable();
    }

    public void IncreaseQuantity(int value = 1)
    {
        quantity += value;
        onQuantityChanged?.Invoke(this, quantity);
    }

    public void DecreaseQuantity(int value = 1)
    {
        quantity -= value;
        onQuantityChanged?.Invoke(this, quantity);
    }

    public int GetQuantity()
    {
        return quantity;
    }

    public Type GetInteractableType()
    {
        return interactable.GetType();
    }

    public T CastTo<T>()
    {
        try
        {
            return (T)interactable;
        }
        catch (System.Exception)
        {
            return default(T);
        }
    }

    public bool IsType<T>(out T output)
    {
        try
        {
            output = (T)interactable;
            return true;
        }
        catch (System.Exception)
        {
            output = default(T);
            return false;
        }
    }

    public bool IsType<T>()
    {
        try
        {
            var output = CastTo<T>();
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}
