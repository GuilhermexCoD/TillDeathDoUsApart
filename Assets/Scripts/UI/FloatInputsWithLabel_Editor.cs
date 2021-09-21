using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FloatInputsWithLabel), true)]
public class FloatInputsWithLabel_Editor : Editor
{
    FloatInputsWithLabel inputs;

    void OnEnable()
    {
        inputs = (FloatInputsWithLabel)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GenerateInputs"))
        {
            inputs.ClearFields();
            inputs.GenerateFloats();
        }
    }

    public override bool RequiresConstantRepaint()
    {
        inputs.UpdateVisual();

        return base.RequiresConstantRepaint();
    }
}
