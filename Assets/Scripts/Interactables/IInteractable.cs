using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    object PickUp(object actor);
    void Interact(object actor);
    string GetInfo();
}
