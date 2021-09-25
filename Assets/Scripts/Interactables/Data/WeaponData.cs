using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapons")]
public class WeaponData : ItemData
{
    [Header("Weapon")]
    public Vector3 handR_Transform;
    public Vector3 handR_Rotation;

    public Vector3 handL_Transform;
    public Vector3 handL_Rotation;

    public Color color = Color.white;
}
