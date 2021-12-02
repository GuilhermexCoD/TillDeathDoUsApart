using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BaseButton : BaseUI
{
    [SerializeField]
    private string _buttonText;

    [SerializeField]
    private TMP_Text _tmpText;

    void Awake()
    {
        UpdateVisual();
    }

    public override void UpdateVisual()
    {
        name = _buttonText;

        if (_tmpText != null)
        {
            _tmpText.text = _buttonText;
        }

        base.UpdateVisual();
    }
}
