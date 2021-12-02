using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager current;

    public GameObject graphVertexPrefab;

    [SerializeField]
    public Transform _sourceTransform;

    [SerializeField]
    public Transform _targetTransform;

    public event Action<GraphManager, EGraphAlgorithm> OnAlgorithmChanged;

    public event Action<int> OnBFS_Executed;
    private int _graphDepth;
    public float _lastPercent;

    //public event Action<int> OnDFS_Executed;

    public event Action<IVertexData<AStarVertexData<Vector2Int>>> OnAStarVertexDataChanged;
    //public event Action<IVertexData<Vector2Int>> OnBFSVertexDataChanged;

    private Graph<Vector2Int, Weight> _graph = new Graph<Vector2Int, Weight>();

    [SerializeField]
    private bool _bCreateEdgesVisual = false;

    private GameObject graphVisuals;
    public List<Vertex<AStarVertexData<Vector2Int>>> aStarPath;

    private void Awake()
    {
        current = Singleton<GraphManager>.Instance;

        if (Level.current != null)
        {
            Level.current.onGenerated += OnLevelGenerated;
            Level.current.onClear += OnLevelClean;

            if (Level.current.IsGenerated())
            {
                OnLevelGenerated(this, new EventArgs());
            }
        }
    }

    private void OnLevelClean()
    {
        if (graphVisuals != null)
        {
            Destroy(graphVisuals);
        }
    }

    public void ToogleVisuals(bool on)
    {
        if (graphVisuals != null)
        {
            graphVisuals.SetActive(on);
        }
    }

    private void OnLevelGenerated(object sender, EventArgs e)
    {
        SetGraphFromMap(Level.current.map);

        SpawnGraphVertex(_graph);

        ToogleVisuals(false);
    }

    public void SetGraphFromMap(HashSet<Vector2Int> map)
    {
        _graph = GetGraphFromMap(map);
    }

    public void ExecuteBFS()
    {
        ExecuteBFS(_sourceTransform);
    }

    public void ExecuteBFS(Vector2Int sourceCoord)
    {
        if (_graph != null)
        {
            OnAlgorithmChanged?.Invoke(this, EGraphAlgorithm.bfs);

            var bfs = new BFS<Vector2Int, Weight>();

            var source = _graph.GetVertex(sourceCoord);

            if (source != null)
            {
                bfs.Execute(_graph, source);

                _graphDepth = bfs.dephLevel;

                OnBFS_Executed?.Invoke(_graphDepth);
            }
            else
            {
                Debug.LogWarning($"Source Coord not a Vertex : {sourceCoord}");
            }
        }
    }

    public void ExecuteBFS(Transform sourceTransform)
    {
        var sourceCoord = Level.PositionToCoord(sourceTransform.transform.position);
        ExecuteBFS(sourceCoord);
    }

    public List<Vector2Int> ExecuteBFS(Transform sourceTransform, float lastPercent)
    {
        var vertices = new List<Vector2Int>();
        float targetPercent = 1 - lastPercent;

        ExecuteBFS(sourceTransform);

        foreach (var vertex in _graph.GetVertices())
        {
            float depthRatio = (float)vertex.GetDistance() / (float)_graphDepth;
            if (depthRatio >= targetPercent)
            {
                vertices.Add(vertex.GetData());
            }
        }
        Debug.Log("finished");
        return vertices;
    }

    public List<Vector2Int> ExecuteBFS(Vector2Int sourceCoord, float lastPercent)
    {
        var vertices = new List<Vector2Int>();
        float targetPercent = 1 - lastPercent;

        ExecuteBFS(sourceCoord);

        foreach (var vertex in _graph.GetVertices())
        {
            float depthRatio = (float)vertex.GetDistance() / (float)_graphDepth;
            if (depthRatio >= targetPercent)
            {
                vertices.Add(vertex.GetData());
            }
        }
        Debug.Log("finished");
        return vertices;
    }

    public void ExecuteDFS()
    {
        ExecuteDFS(_sourceTransform);
    }

    public void ExecuteDFS(Transform sourceTransform)
    {
        if (_graph != null)
        {
            OnAlgorithmChanged?.Invoke(this, EGraphAlgorithm.dfs);

            var dfs = new BFS<Vector2Int, Weight>();

            //TODO make vertex Source more dinamic
            var playerPosition = GameEventsHandler.current.playerGo.transform.position;
            var source = _graph.GetVertex(new Vector2Int((int)playerPosition.x, (int)playerPosition.y));

            dfs.Execute(_graph, source);

            //OnBFS_Executed?.Invoke(dfs.dephLevel);
        }
    }

    public void ExecuteAStar()
    {
        ExecuteAStar(_sourceTransform, _targetTransform);
    }

    public void ExecuteAStar(Vector2Int sourceCoord, Vector2Int targetCoord)
    {
        if (_graph != null)
        {
            OnAlgorithmChanged?.Invoke(this, EGraphAlgorithm.aStar);

            var aStar = new AStar<Vector2Int, Weight>();
            aStar.onVertexDataChanged += AStarOnVertexDataChanged;
            aStar.onPathGenerated += AStarOnPathGenerated;

            var source = _graph.GetVertex(sourceCoord);

            var target = _graph.GetVertex(targetCoord);

            if (source != null && target != null)
            {
                aStar.Execute(_graph, source, target);
            }
            else
            {
                if (source == null)
                    Debug.LogWarning($"{nameof(source)} Coord not a Vertex : {source}");
                if (target == null)
                    Debug.LogWarning($"{nameof(target)} Coord not a Vertex : {target}");
            }
        }
    }

    private void AStarOnPathGenerated(List<Vertex<AStarVertexData<Vector2Int>>> path)
    {
        aStarPath = path;
    }

    public void ExecuteAStar(Transform sourceTransform, Transform targetTransform)
    {
        var sourceCoord = Level.PositionToCoord(sourceTransform.transform.position);
        var targetCoord = Level.PositionToCoord(targetTransform.transform.position);

        ExecuteAStar(sourceCoord, targetCoord);
    }

    private void AStarOnVertexDataChanged(AStarVertexData<Vector2Int> data)
    {
        OnAStarVertexDataChanged?.Invoke(data);
    }

    public Graph<Vector2Int, Weight> GetGraphFromMap(HashSet<Vector2Int> map)
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
                var orientation = direction.Key;

                float weight = GetHypotenuseByOrientation(orientation, 1);
                bool connection = map.Contains(compareCoord);

                if (connection)
                {
                    var connectedVertex = _graph.GetVertex(compareCoord);
                    _graph.AddEdge(vertex, connectedVertex, false, new Weight(weight));
                }
            }
        }

        return _graph;
    }

    private float GetHypotenuseByOrientation(EOrientation orientation, float weight)
    {
        float leg1 = weight;
        float leg2 = weight;
        switch (orientation)
        {
            case EOrientation.Up:
                return weight;
            case EOrientation.Right:
                return weight;
            case EOrientation.Down:
                return weight;
            case EOrientation.Left:
                return weight;
            default:
                break;
        }

        return Util.GetHypotenuse(leg1, leg2);
    }

    private void SpawnGraphVertex(Graph<Vector2Int, Weight> graph)
    {
        graphVisuals = new GameObject($"Graphs");

        foreach (var edgeList in graph.GetEdgeList())
        {
            int vertexIndex = edgeList.Key;
            var vertex = graph.GetVertex(vertexIndex);

            var graphVertex = Instantiate(graphVertexPrefab, Vector3.zero, Quaternion.identity, graphVisuals.transform);
            var vertexUI = graphVertex.GetComponent<WorldGraphVertexUI>();

            vertexUI.OnInitialize(this, vertex);

            foreach (var edge in edgeList.Value)
            {
                int targetIndex = edge.GetVertexIndex();
                var targetVertex = graph.GetVertex(targetIndex);

                var targetWorldPosition = Level.CalculatePosition(targetVertex.GetData());
                vertexUI.AddNeightbour(targetWorldPosition);
            }
            if (_bCreateEdgesVisual)
                vertexUI.CreateEdges();
        }
    }
}
