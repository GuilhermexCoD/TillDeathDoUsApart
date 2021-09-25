using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Interactable
{
    protected DrinkableData drinkableData { get { return GetData<DrinkableData>(); } }
}
