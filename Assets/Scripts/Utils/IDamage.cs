using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void ApplyDamage(GameObject damagedActor, float baseDamage, GameObject damageCauser, DamageType damageType);

}
