using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DFS<V, E> : IGraphAlgorithms<V, E> where V : IEquatable<V> where E : class
{
    public int time = 0;

    public void Execute(Graph<V, E> graph, Vertex<V> source)
    {

        foreach (var vertex in graph.GetVertices())
        {
            vertex.SetVertexColor(ENodeColor.WHITE);
        }

        if (source.GetVertexColor() == ENodeColor.WHITE)
        {
            DFSVisit(graph, source);
        }

    }

    public void Execute(Graph<V, E> graph, Vertex<V> source, Vertex<V> target)
    {
        throw new NotImplementedException();
    }

    private void DFSVisit(Graph<V, E> graph, Vertex<V> vertex)
    {
        time++;
        vertex.SetStartTime(time);
        vertex.SetVertexColor(ENodeColor.GRAY);
        foreach (var edge in graph.GetVertexEdges(vertex))
        {
            var targetVertex = graph.GetVertex(edge.GetVertexIndex());
            if (targetVertex.GetVertexColor() == ENodeColor.WHITE)
            {
                DFSVisit(graph, targetVertex);
            }
        }
        vertex.SetVertexColor(ENodeColor.BLACK);
        time++;
        vertex.SetEndTime(time);
    }
}