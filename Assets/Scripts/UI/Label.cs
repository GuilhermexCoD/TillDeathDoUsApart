using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Label : BaseUI
{
    [SerializeField]
    protected string label;

    [SerializeField]
    protected TMP_Text labelText;

    protected virtual void Awake()
    {
        UpdateVisual();
    }

    public void SetLabel(string label)
    {
        this.label = label;
        UpdateVisual();
    }

    public override void UpdateVisual()
    {
        name = label;
        base.UpdateVisual();

        if (labelText != null)
            labelText.text = label;
    }
}
