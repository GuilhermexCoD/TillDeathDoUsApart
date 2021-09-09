using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    IInteractable PickUp(object actor);
    void Interact(object actor);
    string GetInfo();
}
