using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float health = 100;

    [SerializeField]
    private float maxHealth = 100;

    public event EventHandler<float> OnHealthChanged;

    [SerializeField]
    private bool _bCanDestroy = true;

    // Start is called before the first frame update
    void Awake()
    {
        var actor = this.GetComponent<Actor>();
        if (actor != null)
        {
            actor.OnAnyDamage += OnActorAnyDamage;
        }
    }

    public void OnInitialize(bool bCanDestroy = true)
    {
        this.health = maxHealth;
        this._bCanDestroy = bCanDestroy;  
    }

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float value)
    {
        health = ClampHealth(value);

        OnHealthChanged?.Invoke(this, health);
    }

    private float ClampHealth(float value)
    {
        return Mathf.Clamp(value, 0f, maxHealth);
    }

    public void DecreaseHealth(float value)
    {
        Util.CreateWorldTextPopup($"Health: {health} - {value} = {ClampHealth(health - value)}", this.transform.position, 20, Vector3.one * 0.2f, Color.red, 2, 1);
        SetHealth(health - value);
    }

    public void IncreaseHealth(float value)
    {
        Util.CreateWorldTextPopup($"Health: {health} + {value} = {ClampHealth(health + value)}", this.transform.position, 20, Vector3.one * 0.2f, Color.green, 2, 1);
        SetHealth(health + value);
    }

    private void OnActorAnyDamage(object sender, AnyDamageArgs e)
    {
        DecreaseHealth(e.damageType.ProccessDamage(e.baseDamage));

        if (health <= 0 && _bCanDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        var actor = this.GetComponent<Actor>();
        if (actor != null)
        {
            actor.OnAnyDamage -= OnActorAnyDamage;
        }
    }
}
