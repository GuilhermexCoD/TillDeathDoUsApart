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

        if (GUILayout.Button("Execute BFS with Cut and A*"))
        {
            var coords = graphManager.ExecuteBFS(graphManager._sourceTransform, graphManager._lastPercent);

            var targetCoord = coords[Random.Range(0, coords.Count)];

            var sourceCoord = Level.PositionToCoord(graphManager._sourceTransform.transform.position);

            graphManager.ExecuteAStar(sourceCoord, targetCoord);
        }

        if (GUILayout.Button("Execute AStar"))
        {
            graphManager.ExecuteAStar();
        }
    }
}
#endif

