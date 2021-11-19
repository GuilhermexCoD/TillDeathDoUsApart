using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    public static GraphManager current;

    public GameObject graphVertexPrefab;

    public Transform sourceTransform;

    public Transform targetTransform;

    public event Action<GraphManager, EGraphAlgorithm> OnAlgorithmChanged;

    public event Action<int> OnBFS_Executed;

    //public event Action<int> OnDFS_Executed;

    public event Action<IVertexData<AStarVertexData<Vector2Int>>> OnAStarVertexDataChanged;

    private Graph<Vector2Int, Weight> _graph = new Graph<Vector2Int, Weight>();

    [SerializeField]
    private bool _bCreateEdgesVisual = false;

    [SerializeField]
    private bool _bShowVisuals = false;
    private GameObject graphVisuals;

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

    public void ToogleVisuals()
    {
        if (graphVisuals != null)
        {
            graphVisuals.SetActive(_bShowVisuals);
            _bShowVisuals = !_bShowVisuals;
        }
    }

    private void OnLevelGenerated(object sender, EventArgs e)
    {
        _graph = GetGraphFromMap(Level.current.map);

        SpawnGraphVertex(_graph);

        ToogleVisuals();
    }

    public void ExecuteBFS()
    {
        if (_graph != null)
        {
            OnAlgorithmChanged?.Invoke(this, EGraphAlgorithm.bfs);

            var bfs = new BFS<Vector2Int, Weight>();

            var sourcePosition = Level.PositionToCoord(sourceTransform.transform.position);
            var source = _graph.GetVertex(sourcePosition);

            if (source != null)
            {
                bfs.Execute(_graph, source);

                OnBFS_Executed?.Invoke(bfs.dephLevel);
            }
            else
            {
                Debug.LogWarning($"Source Coord not a Vertex : {sourcePosition}");
            }
        }
    }

    public void ExecuteDFS()
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
        if (_graph != null)
        {
            OnAlgorithmChanged?.Invoke(this, EGraphAlgorithm.aStar);

            var aStar = new AStar<Vector2Int, Weight>();
            aStar.onVertexDataChanged += AStarOnVertexDataChanged;

            var sourcePosition = Level.PositionToCoord(sourceTransform.transform.position);
            var source = _graph.GetVertex(sourcePosition);

            var targetPosition = Level.PositionToCoord(targetTransform.transform.position);
            var target = _graph.GetVertex(targetPosition);

            if (source != null && target != null)
            {
                aStar.Execute(_graph, source, target);
            }
            else
            {
                if (source == null)
                    Debug.LogWarning($"{nameof(source)} Coord not a Vertex : {sourcePosition}");
                if (target == null)
                    Debug.LogWarning($"{nameof(target)} Coord not a Vertex : {targetPosition}");
            }
        }
    }

    private void AStarOnVertexDataChanged(AStarVertexData<Vector2Int> data)
    {
        OnAStarVertexDataChanged?.Invoke(data);
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
