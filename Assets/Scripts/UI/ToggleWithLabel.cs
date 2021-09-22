using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWithLabel : Label
{
    [SerializeField]
    private Toggle toggle;

    public event EventHandler<BoolArgs> onValueChanged;

    protected override void Awake()
    {
        base.Awake();
        toggle?.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool value)
    {
        onValueChanged?.Invoke(this, new BoolArgs()
        {
            value = value
        });
    }

    public bool GetValue()
    {
        return toggle.isOn;
    }
}
