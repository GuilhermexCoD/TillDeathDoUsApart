using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GridSpawner), true)]
public class GridSpawnerEditor : Editor
{
    GridSpawner gridSpawner;

    private void Awake()
    {
        gridSpawner = (GridSpawner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Grid"))
        {
            gridSpawner.SpawnGrid();
        }
    }
}
#endif