using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    float ApplyDamage(Actor damagedActor, float baseDamage, Actor damageCauser, DamageType damageType);
    float ApplyPointDamage(Actor damagedActor, float baseDamage, HitResult hitInfo, Actor damageCauser, DamageType damageType);
    
    event EventHandler<AnyDamageArgs> OnAnyDamage;
    event EventHandler<PointDamageArgs> OnPointDamage;
}
