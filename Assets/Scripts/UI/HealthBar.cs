using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Slider _sliderLostHealth;

    [SerializeField]
    private Slider _sliderActualHealth;

    [SerializeField]
    private Animator _anim;

    private IHealthSystem _healthSystem;

    [Header("Timer")]
    [SerializeField]
    private float _delay;
    private float _timeStampDelay;
    [SerializeField]
    private float _timeToReach;
    private float _timeStampReach;
    [SerializeField]
    private float _timeFade;
    private float _timeStampFade;

    private bool _bIsFading;
    private float _actualValue;
    private float _lostValue;

    [Header("Apperence")]
    [SerializeField]
    private Color _backgroundColor;
    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private Color _lostHealthColor;
    [SerializeField]
    private Image _lostHealthImage;

    [SerializeField]
    private Color _actualHealthColor;
    [SerializeField]
    private Image _actualHealthImage;

    private void Awake()
    {
        OnInitialize();
    }

    private void FixedUpdate()
    {
        if (_timeStampDelay != 0 && Time.time >= _timeStampDelay && Time.time <= _timeStampReach)
        {
            float t = (Time.time - _timeStampDelay) / _timeToReach;
            float lerp = Mathf.Lerp(_lostValue, _actualValue, t);
            _sliderLostHealth.value = lerp;
        }
        else if (_timeStampFade != 0 && Time.time >= _timeStampFade && !_bIsFading)
        {
            _bIsFading = true;
            _anim.SetTrigger("Play");
        }
    }

    private void OnEnable()
    {
        OnInitialize();
    }

    public void OnInitialize()
    {
        _backgroundImage.color = _backgroundColor;
        _lostHealthImage.color = _lostHealthColor;
        _actualHealthImage.color = _actualHealthColor;
        _anim.SetTrigger("Stop");

    }

    public void SetHealthSystem(IHealthSystem healthSystem)
    {
        ClearSubscribedEvents();
        _healthSystem = healthSystem;
        SubscribeToEvents();

        float value = _healthSystem.GetHealthNormalized();

        OnHealthChanged(_healthSystem, new HealthArgs { health = value, causer = null });
    }

    private void SubscribeToEvents()
    {
        _healthSystem.OnHealthChanged += OnHealthChanged;
        _healthSystem.OnHealthEqualsFull += OnHealthFull;
        _healthSystem.OnHealthEqualsZero += OnHealthZero;
    }

    private void ClearSubscribedEvents()
    {
        if (_healthSystem != null)
        {
            _healthSystem.OnHealthChanged -= OnHealthChanged;
            _healthSystem.OnHealthEqualsFull -= OnHealthFull;
            _healthSystem.OnHealthEqualsZero -= OnHealthZero;
        }
    }

    private void OnHealthFull(object sender, EventArgs e)
    {
        //throw new System.NotImplementedException();

    }

    private void OnHealthZero(object sender, EventArgs e)
    {
        //throw new System.NotImplementedException();
    }

    private void OnHealthChanged(object sender, HealthArgs e)
    {
        _sliderActualHealth.value = _healthSystem.GetHealthNormalized();

        _bIsFading = false;
        _timeStampDelay = Time.time + _delay;
        _timeStampReach = _timeStampDelay + _timeToReach;
        _timeStampFade = _timeStampReach + _timeFade;

        _lostValue = _sliderLostHealth.value;
        _actualValue = _sliderActualHealth.value;
    }
}
