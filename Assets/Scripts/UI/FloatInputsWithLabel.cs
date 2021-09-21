using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatInputsWithLabel : Label
{
    [SerializeField]
    private List<float> values;

    public event EventHandler<FloatArrayArgs> onValueChanged;

    [SerializeField]
    private Transform group;

    [SerializeField]
    private GameObject prefabInput;

    private List<GameObject> inputsGo;

    protected override void Awake()
    {
        base.Awake();
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
            print("Child:" + group.childCount);
            for (int i = group.childCount; i > 0; --i)
                DestroyImmediate(group.GetChild(0).gameObject);
        }
    }

    public void GenerateFloats()
    {
        foreach (var value in values)
        {
            var inputGo = Instantiate<GameObject>(prefabInput, group);
            var inputField = inputGo.GetComponent<TMP_InputField>();

            inputField.text = value.ToString();

            if (inputsGo == null)
            {
                inputsGo = new List<GameObject>();
            }

            inputsGo.Add(inputGo);
        }
    }

    private void OnValueChanged(float[] values)
    {
        onValueChanged?.Invoke(this, new FloatArrayArgs()
        {
            values = values
        });
    }
}
