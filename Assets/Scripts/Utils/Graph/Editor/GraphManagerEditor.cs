using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GraphManager), true)]
public class GraphManagerEditor : Editor
{
    GraphManager graphManager;
    void OnEnable()
    {
        graphManager = (GraphManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Toogle Visuals"))
        {
            graphManager.ToogleVisuals();
        }

        if (GUILayout.Button("Execute BFS"))
        {
            graphManager.ExecuteBFS();
        }

        if (GUILayout.Button("Execute AStar"))
        {
            graphManager.ExecuteAStar();
        }
    }
}
#endif

