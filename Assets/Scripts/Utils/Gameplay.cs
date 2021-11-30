using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Gameplay
{
    public static float ApplyDamage(Actor damagedActor, float baseDamage, Actor damageCauser, DamageType damageType)
    {
        float damage = 0;

        if (damagedActor != null)
        {
            damage = damagedActor.ApplyDamage(damagedActor, baseDamage, damageCauser, damageType);
        }

        return damage;
    }

    public static float ApplyPointDamage(Actor damagedActor, float baseDamage, HitResult hitInfo, Actor damageCauser, DamageType damageType)
    {
        float damage = 0;

        if (damagedActor != null)
        {
            damage = damagedActor.ApplyPointDamage(damagedActor, baseDamage, hitInfo, damageCauser, damageType);
        }

        return damage;
    }

    public static float ApplyRadialDamage(Actor damagedActor, float baseDamage, HitResult hitInfo, Actor damageCauser, DamageType damageType)
    {
        float damage = 0;

        if (damagedActor != null)
        {
            damage = damagedActor.ApplyPointDamage(damagedActor, baseDamage, hitInfo, damageCauser, damageType);
        }

        return damage;
    }
}
