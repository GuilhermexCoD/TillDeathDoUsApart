using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Drinkables")]
public class DrinkablesData : ItemData
{
    public EStatusType status;
    public Color liquidColor;

    public float HealAmount;
}
