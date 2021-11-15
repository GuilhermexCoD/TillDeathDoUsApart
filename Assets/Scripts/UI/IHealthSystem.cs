using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthSystem
{
    event EventHandler<HealthArgs> OnHealthChanged;

    event EventHandler OnHealthEqualsFull;

    event EventHandler OnHealthEqualsZero;

    Actor GetActor();

    float GetHealth();

    float GetHealthNormalized();

    void SetHealth(float value, Actor causer);

    void DecreaseHealth(float value, Actor causer);

    void IncreaseHealth(float value, Actor causer);
}
