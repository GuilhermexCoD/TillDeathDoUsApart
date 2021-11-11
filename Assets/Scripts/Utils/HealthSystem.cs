using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IHealthSystem
{
    [SerializeField]
    private float health = 100;

    [SerializeField]
    private float maxHealth = 100;

    public event EventHandler<HealthArgs> OnHealthChanged;

    public event EventHandler OnHealthEqualsFull;

    public event EventHandler OnHealthEqualsZero;

    [SerializeField]
    private bool _bCanDestroy = true;

    private HealthBar _healthBar;

    private Actor _actor;

    // Start is called before the first frame update
    void Awake()
    {
        _actor = this.GetComponent<Actor>();
        if (_actor != null)
        {
            _actor.OnAnyDamage += OnActorAnyDamage;
        }
    }

    public Actor GetActor()
    {
        return _actor;
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

    public float GetHealthNormalized()
    {
        return health / maxHealth;
    }

    public void SetHealth(float value, Actor causer)
    {
        health = ClampHealth(value);

        OnHealthChanged?.Invoke(this, new HealthArgs { health = health, causer = causer });

        if (_healthBar == null)
        {
            var healthUI = PoolingManager.current.GetPooledObject(GameManager.current.healthUiPrefab, this.transform);

            healthUI.transform.localPosition = new Vector3(0, 0.5f);
            _healthBar = healthUI.GetComponentInChildren<HealthBar>();

            _healthBar.SetHealthSystem(this);
        }

        _healthBar.OnInitialize();
    }

    private float ClampHealth(float value)
    {
        return Mathf.Clamp(value, 0f, maxHealth);
    }

    public void DecreaseHealth(float value, Actor causer)
    {
        SetHealth(health - value, causer);
        if (IsDead())
            OnHealthEqualsZero?.Invoke(this, null);
    }

    public void IncreaseHealth(float value, Actor causer)
    {
        SetHealth(health + value, causer);
        if (IsFullHealth())
            OnHealthEqualsFull?.Invoke(this, null);
    }

    private bool IsFullHealth()
    {
        return health == maxHealth;
    }

    private bool IsDead()
    {
        return health == 0;
    }

    private void OnActorAnyDamage(object sender, AnyDamageArgs e)
    {
        DecreaseHealth(e.damageType.ProccessDamage(e.baseDamage), e.damageCauser);

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
