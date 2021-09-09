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
            level.Subscribe();
            level.CleanDebug();
            level.Setup();
            level.UnSubscribe();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("<-"))
        {
            level.DecreaseIteration();
            level.PrintIteration();
        }

        if (GUILayout.Button("PrintIteration"))
        {
            Debug.Log("Print Iteration");
            level.PrintIteration();
        }

        if (GUILayout.Button("->"))
        {
            level.IncreaseIteration();
            level.PrintIteration();
        }
        GUILayout.EndHorizontal();

    }
}