using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageType
{
    public float DamageImpulse;

    public bool CausedByWorld;

    public DamageType(float damageImpulse, bool causedByWorld)
    {
        DamageImpulse = damageImpulse;
        CausedByWorld = causedByWorld;
    }

    public virtual float ProccessDamage(float damage)
    {
        return damage;
    }
}
