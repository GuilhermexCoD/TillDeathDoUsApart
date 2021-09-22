using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
using System.Text.RegularExpressions;

public class InputFieldWithLabel : Label
{
    [SerializeField]
    private string placeHolder = "Enter text...";

    [SerializeField]
    private TMP_Text placeHolderText;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private TMP_InputField.CharacterValidation characterValidation;

    public event EventHandler<TextArgs> onValueChanged;

    protected override void Awake()
    {
        base.Awake();
        inputField?.onValueChanged.AddListener(OnValueChanged);

        SetCharacterValidation(characterValidation);
    }

    public override void UpdateVisual()
    {
        base.UpdateVisual();
        placeHolderText.text = placeHolder;
    }

    private void OnValueChanged(string value)
    {
        Debug.LogWarning($"Changed Value: {value}");

        onValueChanged?.Invoke(this, new TextArgs()
        {
            value = value
        });
    }

    public void SetCharacterValidation(TMP_InputField.CharacterValidation characterValidation)
    {
        this.characterValidation = characterValidation;

        if (inputField != null)
            inputField.characterValidation = characterValidation;
    }

    public string GetValue()
    {
        return inputField.text;
    }

    public void SetValue(string value)
    {
        inputField.text = value;
    }
}
