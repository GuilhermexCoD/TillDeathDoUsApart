using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour, I_BaseUI
{
    protected new string name;

    public virtual void UpdateVisual()
    {

    }

    public virtual void SetGameObjectName(string name)
    {
        this.gameObject.name = $"{Util.RemoveSpecialCharacters(name)}_{this.GetType().Name}";

    }
}
