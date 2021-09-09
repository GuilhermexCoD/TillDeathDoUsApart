using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapons")]
public class WeaponData : ItemData
{
    public ProjectileData Projectile;

    public Color Color;
}
