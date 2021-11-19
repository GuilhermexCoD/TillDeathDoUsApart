using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BFS<V, E> : IGraphAlgorithms<V, E> where V : IEquatable<V> where E : class
{
    public int dephLevel = 0;

    public void Execute(Graph<V, E> graph, Vertex<V> source)
    {

        foreach (var vertex in graph.GetVertices())
        {
            vertex.SetVertexColor(ENodeColor.WHITE);
            vertex.SetDistance(int.MaxValue);
        }

        source.SetVertexColor(ENodeColor.GRAY);
        source.SetDistance(0);
        Queue<Vertex<V>> fila = new Queue<Vertex<V>>();
        fila.Enqueue(source);
        while (fila.Count != 0)
        {
            var vertex = fila.Dequeue();
            foreach (var edge in graph.GetVertexEdges(vertex))
            {
                var vertexEdge = graph.GetVertex(edge.GetVertexIndex());
                if (vertexEdge.GetVertexColor() == ENodeColor.WHITE)
                {
                    vertexEdge.SetVertexColor(ENodeColor.GRAY);
                    vertexEdge.SetDistance(vertex.GetDistance() + 1);
                    fila.Enqueue(vertexEdge);
                }
            }
            vertex.SetVertexColor(ENodeColor.BLACK);
            dephLevel = vertex.GetDistance();
        }
    }

    public void Execute(Graph<V, E> graph, Vertex<V> source, Vertex<V> target)
    {
        throw new NotImplementedException();
    }
}