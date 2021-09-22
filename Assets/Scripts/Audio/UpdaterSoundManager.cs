using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdaterSoundManager : MonoBehaviour
{
    private float value;

    [SerializeField]
    private SliderWithLabel volume;

    private void Awake()
    {
        value = SoundManager.masterVolume;
        volume.onValueChanged += OnVolumeChanged;
    }

    private void OnVolumeChanged(object sender, FloatArgs e)
    {
        SetValue(e.value);
    }

    public void SetValue(float value)
    {
        this.value = value;
        SoundManager.masterVolume = value;
    }

    public float GetValue()
    {
        return value;
    }
}
