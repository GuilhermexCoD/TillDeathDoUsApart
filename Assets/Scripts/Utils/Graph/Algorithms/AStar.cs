using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar<V, E> : IGraphAlgorithms<V, E> where V : IEquatable<V> where E : class
{
    List<Vertex<AStarVertexData<V>>> path = new List<Vertex<AStarVertexData<V>>>();

    public event Action<AStarVertexData<V>> onVertexDataChanged;

    public void Execute(Graph<V, E> graph, Vertex<V> source)
    {
        throw new NotImplementedException();
    }

    public void Execute(Graph<V, E> graph, Vertex<V> source, Vertex<V> target)
    {
        var aStarGraph = ConvertGraphVertexToAStar(graph);

        var sourceAStar = aStarGraph.GetVertex(ConvertVertexToAStarData(source.GetData()));
        var targetAStar = aStarGraph.GetVertex(ConvertVertexToAStarData(target.GetData()));

        var openList = new List<Vertex<AStarVertexData<V>>>();
        var closedList = new HashSet<Vertex<AStarVertexData<V>>>();

        openList.Add(sourceAStar);
        sourceAStar.GetData().SetColor(Color.green);

        while (openList.Count > 0)
        {
            var current = GetLowerFCost(openList);
            openList.Remove(current);
            closedList.Add(current);
            current.GetData().SetColor(Color.red);

            if (current.Equals(targetAStar))
            {
                RetracePath(sourceAStar, targetAStar);
                return;
            }

            var currentEdgeList = aStarGraph.GetVertexEdges(current);

            foreach (var edge in currentEdgeList)
            {

                var neighbour = aStarGraph.GetVertex(edge.GetVertexIndex());

                if (closedList.Contains(neighbour))
                    continue;

                float newMovementCostToNeighbour = current.GetData().GetGCost() + GetDistanceBetweenTwoVertex(current, neighbour);

                if (newMovementCostToNeighbour < neighbour.GetData().GetGCost() || !openList.Contains(neighbour))
                {
                    neighbour.GetData().SetGCost(newMovementCostToNeighbour);
                    neighbour.GetData().SetHCost(GetDistanceBetweenTwoVertex(neighbour, targetAStar));
                    neighbour.GetData().SetParent(current);

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                        neighbour.GetData().SetColor(Color.green);
                    }
                }
            }
        }
    }

    private void RetracePath(Vertex<AStarVertexData<V>> sourceVertex, Vertex<AStarVertexData<V>> targetVertex)
    {
        path = new List<Vertex<AStarVertexData<V>>>();

        var current = targetVertex;

        while (current != sourceVertex)
        {
            path.Add(current);
            current.GetData().SetColor(Color.blue);
            current = current.GetData().GetParent();
        }

        path.Reverse();
    }

    private float GetDistanceBetweenTwoVertex(Vertex<AStarVertexData<V>> vertexA, Vertex<AStarVertexData<V>> vertexB)
    {
        var coordA = (vertexA.GetData().GetVertexData()) as Vector2Int?;
        var coordB = (vertexB.GetData().GetVertexData()) as Vector2Int?;
        float result = -1;
        if (coordA.HasValue && coordB.HasValue)
        {
            result = Vector2Int.Distance(coordA.Value, coordB.Value);
        }

        return result;
    }

    private Vertex<AStarVertexData<V>> GetLowerFCost(List<Vertex<AStarVertexData<V>>> vertices)
    {
        Vertex<AStarVertexData<V>> currentVertex = vertices[0];

        for (int i = 1; i < vertices.Count; i++)
        {
            var targetVertex = vertices[i];
            if (targetVertex.GetData().GetFCost() < currentVertex.GetData().GetFCost() ||
                targetVertex.GetData().GetFCost() == currentVertex.GetData().GetFCost() && targetVertex.GetData().GetHCost() < currentVertex.GetData().GetHCost())
            {
                currentVertex = targetVertex;
            }
        }

        return currentVertex;
    }

    private AStarVertexData<V> ConvertVertexToAStarData(V oldData)
    {
        var aStarData = new AStarVertexData<V>(oldData);
        onVertexDataChanged?.Invoke(aStarData);
        return aStarData;
    }

    private Graph<AStarVertexData<V>, E> ConvertGraphVertexToAStar(Graph<V, E> graph)
    {
        var aStarGraph = new Graph<AStarVertexData<V>, E>();

        foreach (var vertex in graph.GetVertices())
        {
            aStarGraph.AddVertex(vertex.GetLabel(), ConvertVertexToAStarData(vertex.GetData()));
        }

        foreach (var edge in graph.GetEdgeList())
        {
            int vertexIndex = edge.Key;
            foreach (var neightbour in edge.Value)
            {
                int vertexNeightbour = neightbour.GetVertexIndex();

                aStarGraph.AddEdge(vertexIndex, vertexNeightbour, false, neightbour.GetWeight());
            }
        }

        return aStarGraph;
    }
}
