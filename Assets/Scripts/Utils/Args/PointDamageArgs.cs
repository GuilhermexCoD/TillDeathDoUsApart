using System;
using UnityEngine;
public class PointDamageArgs : EventArgs
{
    public float baseDamage;
    public Actor damageCauser;
    public DamageType damageType;
    public HitResult hitInfo;
}
