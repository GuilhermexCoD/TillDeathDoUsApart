using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour, IDamage
{
    public event EventHandler<AnyDamageArgs> OnAnyDamage;
    public event EventHandler<PointDamageArgs> OnPointDamage;
    public event Action<Actor> OnDestroyed;

    public float ApplyDamage(Actor damagedActor, float baseDamage, Actor damageCauser, DamageType damageType)
    {
        OnAnyDamage?.Invoke(this, new AnyDamageArgs
        {
            baseDamage = baseDamage,
            damageCauser = damageCauser,
            damageType = damageType
        });

        float proccessedDamage = damageType.ProccessDamage(baseDamage);

        return proccessedDamage;
    }

    public float ApplyPointDamage(Actor damagedActor, float baseDamage, HitResult hitInfo, Actor damageCauser, DamageType damageType)
    {
        OnAnyDamage?.Invoke(this, new AnyDamageArgs
        {
            baseDamage = baseDamage,
            damageCauser = damageCauser,
            damageType = damageType
        });

        OnPointDamage?.Invoke(this, new PointDamageArgs
        {
            baseDamage = baseDamage,
            damageCauser = damageCauser,
            damageType = damageType,
            hitInfo = hitInfo
        });

        float proccessedDamage = damageType.ProccessDamage(baseDamage);

        return proccessedDamage;
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}
