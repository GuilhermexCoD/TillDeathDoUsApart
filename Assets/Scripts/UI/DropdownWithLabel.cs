using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class DropdownWithLabel : Label
{
    [SerializeField]
    private Enumerator options;

    [SerializeField]
    private TMP_Dropdown dropdown;

    public event EventHandler<EnumArgs> onValueChanged;

    protected override void Awake()
    {
        base.Awake();
        dropdown?.onValueChanged.AddListener(OnDropdownValueChanged);

    }

    public void OnDropdownValueChanged(int value)
    {
        onValueChanged?.Invoke(this, new EnumArgs()
        {
            value = value
        });
    }

    public override void UpdateVisual()
    {

        base.UpdateVisual();

        if (dropdown != null)
        {
            dropdown.ClearOptions();

            var optionsList = options?.GetNames().ToList();
            if (optionsList != null)
            {
                dropdown.AddOptions(optionsList);
            }
        }
    }
    public int GetValue()
    {
        return dropdown.value;
    }
}
