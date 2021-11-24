using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterSoundManager : MonoBehaviour
{
    private float _value;

    private SliderWithLabel _volume;

    private void Awake()
    {
        _volume = this.GetComponent<SliderWithLabel>();
        _value = _volume.GetValue();
        SoundManager.SetMasterVolume(_value);
        _volume.onValueChanged += OnVolumeChanged;
    }

    private void OnVolumeChanged(object sender, float value)
    {
        SetValue(value);
    }

    public void SetValue(float value)
    {
        this._value = value;
        SoundManager.SetMasterVolume(value);
    }

    public float GetValue()
    {
        return _value;
    }
}
