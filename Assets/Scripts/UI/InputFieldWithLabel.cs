using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using System.Text.RegularExpressions;

public class InputFieldWithLabel : BaseUI_Label 
{
    [SerializeField]
    private string placeHolder = "Enter text...";

    [SerializeField]
    private TMP_Text placeHolderText;

    [SerializeField]
    private TMP_InputField inputField;

    public event EventHandler<TextArgs> onValueChanged;

    private void Awake()
    {
        inputField?.onValueChanged.AddListener(OnValueChanged);
    }

    public override void UpdateVisual()
    {
        base.UpdateVisual();
        placeHolderText.text = placeHolder;
    }

    private void OnValueChanged(string value){
        onValueChanged?.Invoke(this, new TextArgs() 
        { 
            value = value 
        });
    }

    public string GetInputFieldValue()
    {
        return inputField.text;
    }
}
