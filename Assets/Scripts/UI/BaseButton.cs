using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseButton : BaseUI
{
    [SerializeField]
    private string buttonText;

    [SerializeField]
    private TMP_Text text;

    protected virtual void Awake()
    {
        UpdateVisual();
    }

    public override void UpdateVisual()
    {
        name = buttonText;
        base.UpdateVisual();

        if (text != null)
        {
            text.text = buttonText;
        }
    }
}
