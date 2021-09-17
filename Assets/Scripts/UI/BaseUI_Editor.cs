using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BaseUI), true)]
public class BaseUI_Editor : Editor
{
    BaseUI baseUI;

    void OnEnable()
    {
        baseUI = (BaseUI)target;

    }

    public override bool RequiresConstantRepaint()
    {
        baseUI.UpdateVisual();

        return base.RequiresConstantRepaint();
    }
}
