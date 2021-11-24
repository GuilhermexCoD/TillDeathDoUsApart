using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(BaseUI), true)]
[CanEditMultipleObjects]
public class BaseUI_Editor : Editor
{
    //BaseUI baseUI;
    List<BaseUI> baseUIs = new List<BaseUI>();

    void OnEnable()
    {
        foreach (var tar in targets)
        {
            baseUIs.Add((BaseUI)tar);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Visuals"))
        {
            foreach (var baseUI in baseUIs)
            {
                baseUI.UpdateVisual();
            }
        }
    }

    //public override bool RequiresConstantRepaint()
    //{


    //    return base.RequiresConstantRepaint();
    //}
}
#endif
