using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseUI_Label : BaseUI
{
    [SerializeField]
    protected string label;

    [SerializeField]
    protected TMP_Text labelText;

    protected virtual void Awake()
    {
        UpdateVisual();
    }

    public override void UpdateVisual()
    {
        base.UpdateVisual();

        if (labelText != null)
            labelText.text = label;

        this.gameObject.name = $"{Util.RemoveSpecialCharacters(label)}_{this.GetType().Name}";
    }
}
