using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithLabel : Label
{
    public event EventHandler<float> onValueChanged;
    public Slider slider;

    protected override void Awake()
    {
        Debug.Log("Awake Slider");
        base.Awake();
        if (slider != null)
        {
            Debug.Log("Slider valid");
            slider?.onValueChanged.AddListener(OnSliderChangedValue);
        }
        else
        {
            Debug.LogError("Slider INVALID");
        }
    }

    public void OnSliderChangedValue(float value)
    {
        onValueChanged?.Invoke(this, value);
    }

    public override void UpdateVisual()
    {
        base.UpdateVisual();
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }

    public float GetValue()
    {
        return slider.value;
    }

}
