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

        if (GUILayout.Button("Execute BFS"))
        {
            graphManager.ExecuteBFS();
        }
    }
}
#endif

