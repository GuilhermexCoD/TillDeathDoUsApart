using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level),true)]
public class LevelEditor : Editor
{
    Level level;

    private void Awake()
    {
        level = (Level)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Dungeon"))
        {
            level.Setup();
        }
    }
}