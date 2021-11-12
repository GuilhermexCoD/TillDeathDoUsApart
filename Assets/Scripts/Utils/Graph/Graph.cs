using System;
using System.Collections.Generic;
using System.Linq;

public class Graph
{
    public static int GetEdgeCountForCompleteGraph(int vertexCount)
    {
        return (vertexCount - 1) * vertexCount / 2;
    }
}

public class Graph<V, E> where E : class
{
    private List<Vertex<V>> vertices;
    private Dictionary<int, List<Edge<E>>> edgesList;

    public Graph()
    {
        vertices = new List<Vertex<V>>();
        edgesList = new Dictionary<int, List<Edge<E>>>();
    }

    public Graph(List<Vertex<V>> vertices)
    {
        this.vertices = vertices;
        edgesList = new Dictionary<int, List<Edge<E>>>();
    }

    public Graph(List<Vertex<V>> vertices, Dictionary<int, List<Edge<E>>> edgesList)
    {
        this.vertices = vertices;
        this.edgesList = edgesList;
    }

    public int AddVertex(string label, V data)
    {
        int index = vertices.Count;

        vertices.Add(new Vertex<V>(label, data));

        return index;
    }

    public int AddVertex(V data)
    {
        int index = vertices.Count;

        vertices.Add(new Vertex<V>(data));

        return index;
    }

    public void RemoveVertex(int index)
    {
        RemoveVertexConnections(index);
        vertices.RemoveAt(index);
    }

    public void RemoveVertex(Vertex<V> vertex)
    {
        RemoveVertexConnections(vertex);
        vertices.Remove(vertex);
    }

    public void RemoveVertexConnections(Vertex<V> vertex)
    {
        RemoveVertexConnections(GetVertexIndex(vertex));
    }

    public void RemoveVertexConnections(int index)
    {
        edgesList.Remove(index);
        foreach (var edge in edgesList)
        {
            edge.Value.RemoveAll(e => e.IsConnectingVertex(index));
        }
    }

    public List<Edge<E>> GetEdgeIndexes(int keyIndex)
    {
        List<Edge<E>> indexes = new List<Edge<E>>();

        edgesList.TryGetValue(keyIndex, out indexes);

        return indexes;
    }

    public void RemoveEdge(Vertex<V> a, Vertex<V> b)
    {
        RemoveEdge(GetVertexIndex(a), GetVertexIndex(b));
    }

    public void RemoveEdge(int indexA, int indexB)
    {
        var edges = GetEdgeIndexes(indexA);

        edges.RemoveAll(e => e.IsConnectingVertex(indexB));
    }

    public void AddEdge(int indexA, int indexB, bool directed = true, E weight = null)
    {
        if (directed)
        {
            AddEdgeConnection(indexA, indexB, weight);
        }
        else
        {
            AddEdgeConnection(indexA, indexB, weight);
            AddEdgeConnection(indexB, indexA, weight);
        }
    }

    public void AddEdge(Vertex<V> a, Vertex<V> b, bool directed = true, E weight = null)
    {
        if (directed)
        {
            AddEdgeConnection(a, b, weight);
        }
        else
        {
            AddEdgeConnection(a, b, weight);
            AddEdgeConnection(b, a, weight);
        }
    }

    public void AddEdgeConnection(int indexA, int indexB, E weight = null)
    {
        var edgeIndexes = new List<Edge<E>>();

        var edgeAB = new Edge<E>(indexA, indexB, weight);

        if (edgesList.ContainsKey(indexA))
        {
            var success = edgesList.TryGetValue(indexA, out edgeIndexes);

            if (success && !edgeIndexes.Any(e => e.IsConnectingVertex(indexB)))
            {
                edgeIndexes.Add(edgeAB);
            }
        }
        else
        {
            edgeIndexes.Add(edgeAB);
            edgesList.Add(indexA, edgeIndexes);
        }
    }

    public void AddEdgeConnection(Vertex<V> a, Vertex<V> b, E weight = null)
    {
        AddEdgeConnection(GetVertexIndex(a), GetVertexIndex(b), weight);
    }

    public int GetVertexIndex(Vertex<V> vertex)
    {
        return vertices.IndexOf(vertex);
    }

    public Vertex<V> GetVertex(V data)
    {
        foreach (var vertex in vertices)
        {
            if (vertex.GetData().GetHashCode() == data.GetHashCode())
            {
                return vertex;
            }
        }

        return null;
    }

    public Vertex<V> GetVertex(int index)
    {
        return vertices[index];
    }

    public List<Edge<E>> GetVertexEdges(int vertexIndex)
    {
        List<Edge<E>> edges = new List<Edge<E>>();

        if (edgesList.ContainsKey(vertexIndex))
        {
            var success = edgesList.TryGetValue(vertexIndex, out edges);
        }

        return edges;
    }

    public List<Edge<E>> GetVertexEdges(Vertex<V> vertex)
    {
        int index = GetVertexIndex(vertex);

        return GetVertexEdges(index);
    }

    public bool IsVertexLinkedTo(int indexA, int indexB)
    {
        bool result = false;

        List<Edge<E>> edges = GetVertexEdges(indexA);

        result = edges.Any(e => e.IsConnectingVertex(indexB));

        return result;
    }

    public bool IsVertexLinkedTo(Vertex<V> a, Vertex<V> b)
    {
        int indexA = GetVertexIndex(a);
        int indexB = GetVertexIndex(b);

        return IsVertexLinkedTo(indexA, indexB);
    }

    public int GetAmountVertexConnections(int index)
    {
        List<Edge<E>> edges = GetVertexEdges(index);

        return edges.Count;
    }

    public int GetAmountVertexConnections(Vertex<V> vertex)
    {
        int index = GetVertexIndex(vertex);

        return GetAmountVertexConnections(index);
    }

    public List<Vertex<V>> GetVertices()
    {
        return vertices;
    }

    public Dictionary<int, List<Edge<E>>> GetEdgeList()
    {
        return edgesList;
    }

    public void ConnectAllVertices()
    {
        int count = vertices.Count();

        for (int i = 0; i < count; i++)
        {
            for (int j = i + 1; j < count; j++)
            {
                AddEdge(i, j);
            }
        }
    }

    public static Graph<V, E> ListToGraph(List<V> list)
    {
        Graph<V, E> graph = new Graph<V, E>();

        foreach (var item in list)
        {
            graph.AddVertex(item);
        }

        return graph;
    }

    public int GetSize()
    {
        return vertices.Count;
    }

    public bool[] DepthFirstSearch()
    {
        int size = GetSize();

        bool[] visited = new bool[size];

        int currentVertex = 0;

        while (currentVertex < size && !visited[currentVertex])
        {
            DepthFirstSearch(currentVertex, ref visited);
            currentVertex++;
        }

        return visited;
    }

    public void DepthFirstSearch(int vertexIndex, ref bool[] visited)
    {
        if (visited[vertexIndex])
            return;

        visited[vertexIndex] = true;

        var neightbours = GetVertexEdges(vertexIndex);

        foreach (var next in neightbours)
        {
            DepthFirstSearch(next.GetVertexIndex(), ref visited);
        }
    }
}
