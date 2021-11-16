using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DFS : MonoBehaviour 
{
    int time = 0;

    public void Execute<V, E>(Graph<V, E> graph, Vertex<V> source) where E : class where V : IEquatable<V>
    {
        
        foreach(var vertex in graph.GetVertices())
        {
            vertex.SetVertexColor(ENodeColor.WHITE);
        }

        if(source.GetVertexColor() == ENodeColor.WHITE)
        {
           DFSVisit(graph, source);
        }
        
    }

    private void DFSVisit<V, E>(Graph<V, E> graph, Vertex<V> vertex)
        where V : IEquatable<V>
        where E : class
    {
        time++;
        vertex.SetStartTime(time);
        vertex.SetVertexColor(ENodeColor.GRAY);
        foreach(var edge in graph.GetVertexEdges(vertex))
        {
            var targetVertex = graph.GetVertex(edge.GetVertexIndex());
            if(targetVertex.GetVertexColor() == ENodeColor.WHITE)
            {
                DFSVisit(graph, targetVertex);
            }
        }
        vertex.SetVertexColor(ENodeColor.BLACK);
        time++;
        vertex.SetEndTime(time);
    }

    private void Start()
    {
        var playerPosition = GameEventsHandler.current.playerGo.transform.position;

        var graph = Level.current.graph;
        var source = graph.GetVertex(new Vector2Int((int)playerPosition.x, (int)playerPosition.y));

        this.Execute(graph, source);

        Debug.Log($"{source.GetStartTime()} / {source.GetEndTime()}");
    }
}