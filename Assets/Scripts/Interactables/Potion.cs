using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IInteractable
{
    public EStatusType effectType;

    public DrinkablesData data;

    public void Interact(object actor)
    {
        Debug.Log(this.ToString());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Colidiu {collision.gameObject.name}");
    }

    public override string ToString()
    {
        return $"Potion : {effectType} - {data}";
    }

    public string GetInfo()
    {
        return this.ToString();
    }

    public IInteractable PickUp(object actor)
    {
        Destroy(this.gameObject);

        return this;
    }

    public bool IsStackable()
    {
        return true;
    }
}
