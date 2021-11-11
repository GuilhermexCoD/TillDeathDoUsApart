using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHealthOverTime : MonoBehaviour
{
    private HealthSystem _healthSystem;
    private float _amount;
    private float _seconds;
    private Actor _causer;
    private bool _bisHealing;
    private Coroutine changeCoroutine;
    public void OnInitialize(Actor causer, float amount, float seconds, bool bHealing)
    {
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnHealthChanged += OnHealthChanged;
        _causer = causer;
        _amount = amount;
        _seconds = seconds;
        _bisHealing = bHealing;

        changeCoroutine = StartCoroutine(Change());
    }

    private void OnDestroy()
    {
        _healthSystem.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(object sender, HealthArgs e)
    {
        if (e.health <= 0 || _healthSystem.GetHealthNormalized() == 1)
        {
            if (changeCoroutine != null)
            {
                StopCoroutine(changeCoroutine);
            }

            Destroy(this);
        }
    }

    private IEnumerator Change()
    {
        while (true)
        {
            if (_bisHealing)
            {
                _healthSystem.IncreaseHealth(_amount, _causer);
            }
            else
            {
                Gameplay.ApplyDamage(_healthSystem.GetActor(), _amount, _causer, new DamageType(0f, true));
            }

            yield return new WaitForSeconds(_seconds);
        }
    }
}
