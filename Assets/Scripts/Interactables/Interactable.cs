using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected ItemData data;

    public event EventHandler<InteractableArgs> onPickedUp;
    public event EventHandler<InteractableArgs> onInteracted;

    public D GetData<D>() where D : ItemData
    {
        return (D)data;
    }

    public int GetId()
    {
        return data.GetId();
    }

    public string GetInfo()
    {
        return data.ToString();
    }

    public int GetQuantity()
    {
        return data.quantity;
    }

    public void Interact(object instigator)
    {
        onInteracted?.Invoke(this, new InteractableArgs { instigator = instigator });
    }

    public bool IsStackable()
    {
        return data.stackable;
    }

    public IInteractable PickUp(object instigator)
    {
        onPickedUp?.Invoke(this, new InteractableArgs { instigator = instigator});
        return this;
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }

    public void SetData<D>(D data) where D : ItemData
    {
        this.data = data;
    }
}
