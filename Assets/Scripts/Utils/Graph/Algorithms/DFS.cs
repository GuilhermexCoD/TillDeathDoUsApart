using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DFS : MonoBehaviour 
{
    public void Execute<V, E>(Graph<V, E> graph) where E : class where V : IEquatable<V>
    {
        int time = 0;
        foreach(var vertex in graph.GetVertices())
        {
            vertex.SetVertexColor(ENodeColor.WHITE);
        }

        foreach (var vertex in graph.GetVertices())
        {
            
        }
    }

    private void Start()
    {
        var graph = Level.current.graph;

        this.Execute(graph);
    }
}