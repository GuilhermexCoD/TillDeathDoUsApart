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

        if (GUILayout.Button("Clear"))
        {
            gridSpawner.ClearChilds();
        }

        if (GUILayout.Button("Create Grid"))
        {
            gridSpawner.ClearChilds();
            gridSpawner.SpawnGrid();
        }

        if (GUILayout.Button("Generate"))
        {
            for (int i = 0; i < gridSpawner.transform.childCount; i++)
            {
                gridSpawner.transform.GetChild(i)?.GetComponent<TrainingManager>()?.Setup();
            }
        }
    }
}
#endif