using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    IInteractable PickUp(object instigator);
    void Interact(object instigator);
    string GetInfo();
    bool IsStackable();
    int GetId();
    D GetData<D>() where D : ItemData;
    void SetData<D>(D data) where D : ItemData;
    int GetQuantity();
}
