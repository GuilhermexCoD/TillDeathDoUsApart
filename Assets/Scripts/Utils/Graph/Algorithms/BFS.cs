using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BFS : IGraphAlgorithms
{
    public int dephLevel = 0;

    public void Execute<V, E>(Graph<V, E> graph, Vertex<V> source) where E : class where V : IEquatable<V>
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
}