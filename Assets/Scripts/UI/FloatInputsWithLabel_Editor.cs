using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

[CustomEditor(typeof(FloatInputsWithLabel), true)]
public class FloatInputsWithLabel_Editor : Editor
{
    FloatInputsWithLabel inputs;

    float[] lastValues;

    void OnEnable()
    {
        inputs = (FloatInputsWithLabel)target;
        lastValues = inputs.GetValues().ToArray();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GenerateInputs") && HasChanged())
        {
            inputs.ClearFields();
            inputs.GenerateFloats();
            lastValues = inputs.GetValues().ToArray();
        }
    }

    private bool HasChanged()
    {
        var lastValuesList = lastValues.ToList();
        var values = inputs.GetValues();

        if (lastValuesList.Count != values.Count)
            return true;


        for (int i = 0; i < lastValues.Length; i++)
        {
            if (values[i] != lastValues[i])
                return true;
        }

        return false;
    }

    public override bool RequiresConstantRepaint()
    {
        inputs.UpdateVisual();

        return base.RequiresConstantRepaint();
    }
}

#endif