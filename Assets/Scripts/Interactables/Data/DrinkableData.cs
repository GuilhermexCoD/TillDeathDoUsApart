using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Drinkables")]
public class DrinkableData : ItemData
{
    [Header("Drinkable")]
    public EStatusType status;

    public Color liquidColor;

    public float amount;

    public override string ToString()
    {
        return base.ToString() + $"Potion : {status}"; ;
    }
}
