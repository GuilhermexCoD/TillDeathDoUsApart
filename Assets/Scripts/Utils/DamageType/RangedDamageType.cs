using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangedDamageType : DamageType
{
    public RangedDamageType(float damageImpulse, bool causedByWorld) : base(damageImpulse, causedByWorld)
    {
    }

    public override float ProccessDamage(float damage)
    {
        return base.ProccessDamage(damage);
    }
}
