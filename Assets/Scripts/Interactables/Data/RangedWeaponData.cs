using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New RangedWeapon", menuName = "Items/Weapons/Ranged")]
public class RangedWeaponData : WeaponData
{
    [Header("Ranged Weapon")]
    public ProjectileData projectile;

    public int magazineSize; 
    public float reloadTime;
}
