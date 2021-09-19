using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DropdownWithLabel), true)]
public class DropdownWithLabel_Editor : Editor
{
    DropdownWithLabel dropdown;

    void OnEnable()
    {
        dropdown = (DropdownWithLabel)target;
    }

    public override bool RequiresConstantRepaint()
    {
        dropdown.UpdateVisual();

        return base.RequiresConstantRepaint();
    }
}
