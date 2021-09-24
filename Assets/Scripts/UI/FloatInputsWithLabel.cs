using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatInputsWithLabel : Label
{
    [SerializeField]
    private List<string> labels;
    [SerializeField]
    private List<float> values;

    public event EventHandler<FloatArrayArgs> onValueChanged;

    [SerializeField]
    private Transform group;

    [SerializeField]
    private GameObject prefabInput;

    [SerializeField]
    private TMP_InputField.CharacterValidation characterValidation;

    public List<InputFieldWithLabel> inputFields;

    protected override void Awake()
    {
        base.Awake();
        inputFields = new List<InputFieldWithLabel>();
        ClearFields();
        GenerateFloats();
    }

    public override void UpdateVisual()
    {
        base.UpdateVisual();
    }

    public void ClearFields()
    {
        if (group != null)
        {
            for (int i = group.childCount; i > 0; --i)
                DestroyImmediate(group.GetChild(0).gameObject);
        }
    }

    public void GenerateFloats()
    {
        for (int i = 0; i < values.Count; i++)
        {
            var value = values[i];
            var inputGo = Instantiate<GameObject>(prefabInput, group);
            var inputField = inputGo.GetComponent<InputFieldWithLabel>();

            inputField.SetValue(value.ToString());
            inputField.SetCharacterValidation(characterValidation);

            inputFields.Add(inputField);
            inputField.onValueChanged += OnInputValueChanged;

            if (i < labels.Count)
            {
                inputField.SetLabel(labels[i]);
            }
            else
            {
                inputField.SetLabel(string.Empty);
            }

        }
    }

    private void OnInputValueChanged(object sender, TextArgs e)
    {
        int index = inputFields.FindIndex(input => input == (InputFieldWithLabel)sender);

        float.TryParse(e.value, out float result);

        values[index] = result;

        OnValueChanged(values.ToArray());
    }

    public List<float> GetValues()
    {
        return values;
    }

    private void OnValueChanged(float[] values)
    {
        onValueChanged?.Invoke(this, new FloatArrayArgs()
        {
            values = values
        });
    }
}
