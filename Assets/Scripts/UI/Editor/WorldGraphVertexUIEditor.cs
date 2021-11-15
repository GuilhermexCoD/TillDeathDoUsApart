using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(WorldGraphVertexUI), true)]
public class WorldGraphVertexUIEditor : Editor
{
    WorldGraphVertexUI graphVertex;

    void OnEnable()
    {
        graphVertex = (WorldGraphVertexUI)target;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Edges"))
        {
            graphVertex.CreateEdges();
        }
    }
}
#endif

