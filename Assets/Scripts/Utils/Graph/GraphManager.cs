using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager current;

    public GameObject graphVertexPrefab;

    public event Action<GraphManager, EGraphAlgorithm> OnAlgorithmChanged;

    public event Action<int> OnBFS_Executed;

    //public event Action<int> OnDFS_Executed;

    private Graph<Vector2Int, Weight> _graph = new Graph<Vector2Int, Weight>();

    private void Awake()
    {
        current = Singleton<GraphManager>.Instance;

        if (Level.current != null)
        {
            Level.current.onGenerated += OnLevelGenerated;

            if (Level.current.IsGenerated())
            {
                OnLevelGenerated(this, new EventArgs());
            }
        }
    }

    private void OnLevelGenerated(object sender, EventArgs e)
    {
        _graph = GetGraphFromMap(Level.current.map);

        SpawnGraphVertex(_graph);
    }

    public void ExecuteBFS()
    {
        if (_graph != null)
        {
            OnAlgorithmChanged?.Invoke(this, EGraphAlgorithm.bfs);

            var bfs = new BFS();

            //TODO make vertex Source more dinamic
            var playerPosition = GameEventsHandler.current.playerGo.transform.position;
            var source = _graph.GetVertex(new Vector2Int((int)playerPosition.x, (int)playerPosition.y));

            bfs.Execute(_graph, source);

            OnBFS_Executed?.Invoke(bfs.dephLevel);
        }
    }

    public void ExecuteDFS()
    {
        if (_graph != null)
        {
            OnAlgorithmChanged?.Invoke(this, EGraphAlgorithm.dfs);

            var dfs = new BFS();

            //TODO make vertex Source more dinamic
            var playerPosition = GameEventsHandler.current.playerGo.transform.position;
            var source = _graph.GetVertex(new Vector2Int((int)playerPosition.x, (int)playerPosition.y));

            dfs.Execute(_graph, source);

            //OnBFS_Executed?.Invoke(dfs.dephLevel);
        }
    }

    private Graph<Vector2Int, Weight> GetGraphFromMap(HashSet<Vector2Int> map)
    {
        _graph = new Graph<Vector2Int, Weight>();

        foreach (var coord in map)
        {
            _graph.AddVertex($"{coord.x}_{coord.y}", coord);
        }

        foreach (var vertex in _graph.GetVertices())
        {
            var coord = vertex.GetData();
            foreach (var direction in Direction2D.EightDirections)
            {
                Vector2Int compareCoord = coord + direction.Value;

                bool connection = map.Contains(compareCoord);

                if (connection)
                {
                    var connectedVertex = _graph.GetVertex(compareCoord);
                    _graph.AddEdge(vertex, connectedVertex, false, new Weight(1));
                }
            }
        }

        return _graph;
    }

    private void SpawnGraphVertex(Graph<Vector2Int, Weight> graph)
    {
        var parent = new GameObject($"Graphs");

        foreach (var edgeList in graph.GetEdgeList())
        {
            int vertexIndex = edgeList.Key;
            var vertex = graph.GetVertex(vertexIndex);

            var graphVertex = Instantiate(graphVertexPrefab, Vector3.zero, Quaternion.identity, parent.transform);
            var vertexUI = graphVertex.GetComponent<WorldGraphVertexUI>();

            vertexUI.OnInitialize(this, vertex);

            foreach (var edge in edgeList.Value)
            {
                int targetIndex = edge.GetVertexIndex();
                var targetVertex = graph.GetVertex(targetIndex);

                var targetWorldPosition = Level.CalculatePosition(targetVertex.GetData());
                vertexUI.AddNeightbour(targetWorldPosition);
            }
            vertexUI.CreateEdges();
        }
    }
}
