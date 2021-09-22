using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithLabel : Label
{
    public event EventHandler<FloatArgs> onValueChanged;
    public Slider slider;

    protected override void Awake()
    {
        base.Awake();
        slider?.onValueChanged.AddListener(OnSliderChangedValue);
    }

    public void OnSliderChangedValue(float value)
    {
        onValueChanged?.Invoke(this, new FloatArgs()
        {
            value = value
        });
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
