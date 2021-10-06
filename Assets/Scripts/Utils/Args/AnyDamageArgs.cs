using System;
using UnityEngine;

public class AnyDamageArgs : EventArgs
{
    public float baseDamage;
    public Actor damageCauser;
    public DamageType damageType;
}
