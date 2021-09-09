using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageType
{
    public float DamageImpulse;

    public bool CausedByWorld;

    public virtual float ProccessDamage(float damage)
    {
        return damage;
    }
}
