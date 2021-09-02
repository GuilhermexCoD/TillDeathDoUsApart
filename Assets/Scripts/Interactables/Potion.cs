using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IInteractable
{
    public float HealAmount = 10;
    public float Price = 5;

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
        return $"Potion : Heal amount = {HealAmount} / Price = {Price}";
    }

    public string GetInfo()
    {
        return this.ToString();
    }

    public object PickUp(object actor)
    {
        Destroy(this.gameObject);

        return this;
    }
}
