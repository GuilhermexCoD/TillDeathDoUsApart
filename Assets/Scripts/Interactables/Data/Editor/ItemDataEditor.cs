using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    ItemData data;

    private void Awake()
    {
        data = (ItemData)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        string id = (string.IsNullOrWhiteSpace(data.name)) ? "Must set name" : data.GetId().ToString();
        GUILayout.Label($"id: {id}");
    }
}
#endif
