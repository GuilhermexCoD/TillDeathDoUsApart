using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldGraphVertexUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _coordText;

    [Header("BFS")]
    [SerializeField]
    private GameObject _bfsParent;
    [SerializeField]
    private TextMeshProUGUI _depthText;

    [Header("A Star")]
    [SerializeField]
    private GameObject _aStarParent;

    [SerializeField]
    private TextMeshProUGUI _gCostText;

    [SerializeField]
    private TextMeshProUGUI _hCostText;

    [SerializeField]
    private TextMeshProUGUI _fCostText;

    [SerializeField]
    private GameObject _parentDirection;

    [Header("Graph")]

    private IVertexData<AStarVertexData<Vector2Int>> aStarData;

    private Vertex<Vector2Int> vertex;

    [SerializeField]
    private Vector2Int coord;

    private string value;

    private int _lastValue;
    private int _total;

    [Header("Apperance")]

    [SerializeField]
    private GameObject edgePrefab;

    [SerializeField]
    private Image _vertexNode;

    public Gradient gradient;
    [SerializeField]
    private Color lineColor;
    [SerializeField]
    private float edgeSize = 10;
    [SerializeField]
    private Transform edgeParent;
    private List<Vector2> neightbours = new List<Vector2>();
    private List<GameObject> edges = new List<GameObject>();

    private GraphManager _graphManager;
    public void OnInitialize(GraphManager graphManager, Vertex<Vector2Int> vertex)
    {
        _graphManager = graphManager;
        _graphManager.OnAlgorithmChanged += OnAlgorithmChanged;
        SetVertex(vertex);
    }

    private void OnAStarVertexDataChanged(IVertexData<AStarVertexData<Vector2Int>> vertexData)
    {
        if (vertexData.GetData().GetVertexData().Equals(coord))
        {
            aStarData = vertexData.GetData();

            aStarData.GetData().onGCostChanged += OnGCostChanged;
            OnGCostChanged(aStarData.GetData().GetGCost());
            aStarData.GetData().onHCostChanged += OnHCostChanged;
            OnGCostChanged(aStarData.GetData().GetHCost());

            aStarData.GetData().onColorChanged += OnColorChanged;
            OnColorChanged(aStarData.GetData().GetColor());

            aStarData.GetData().onParentChanged += OnParentChanged;
            OnParentChanged(aStarData.GetData().GetParent());
        }
    }

    private void OnParentChanged(Vertex<AStarVertexData<Vector2Int>> parent)
    {
        if (parent != null)
        {
            _parentDirection.SetActive(true);

            Vector2Int endPosition = parent.GetData().GetVertexData();

            Vector2Int direction = endPosition - coord;

            float angle = Util.GetAngleFromVectorFloat(direction);
            _parentDirection.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            _parentDirection.SetActive(false);
        }
        
    }

    private void OnColorChanged(Color color)
    {
        SetVertexNodeColor(color);
    }

    private void OnGCostChanged(float gCost)
    {
        _gCostText.text = gCost.ToString("0.00");
        UpdateFCost();
    }

    private void OnHCostChanged(float hCost)
    {
        _hCostText.text = hCost.ToString("0.00");
        UpdateFCost();
    }

    private void UpdateFCost()
    {
        _fCostText.text = aStarData?.GetData().GetFCost().ToString("0.00");
    }

    public void SetVertex(Vertex<Vector2Int> vertex)
    {
        this.vertex = vertex;

        this.vertex.onDistanceChanged += OnDistanceChanged;

        SetCoord(vertex.GetData());
    }

    private void OnAlgorithmChanged(GraphManager graphManager, EGraphAlgorithm algorithm)
    {
        UnSubscribeGraphManager(graphManager);

        SetMethodVisualsFalse();

        switch (algorithm)
        {
            case EGraphAlgorithm.bfs:
                graphManager.OnBFS_Executed += OnBFS_Executed;
                _bfsParent.SetActive(true);
                break;
            case EGraphAlgorithm.dfs:
                break;
            case EGraphAlgorithm.aStar:
                _aStarParent.SetActive(true);
                graphManager.OnAStarVertexDataChanged += OnAStarVertexDataChanged;
                break;
            default:
                break;
        }
    }

    private void SetMethodVisualsFalse()
    {
        _bfsParent.SetActive(false);
        _aStarParent.SetActive(false);
    }

    private void UnSubscribeGraphManager(GraphManager graphManager)
    {
        graphManager.OnBFS_Executed -= OnBFS_Executed;
        graphManager.OnAStarVertexDataChanged -= OnAStarVertexDataChanged;
    }

    private void OnBFS_Executed(int distance)
    {
        _total = distance;
        UpdateColor(_lastValue);
        UpdateLabel(_lastValue, _total);
    }

    private void OnDistanceChanged(int value)
    {
        _lastValue = value;
    }

    private float GetNormalizedValue(int value)
    {
        return (float)value / (float)_total;
    }

    private void UpdateColor(int value)
    {
        var color = gradient.Evaluate(GetNormalizedValue(value));

        SetVertexNodeColor(color);
    }

    public void UpdateLabel(int value, int max)
    {
        string result = $"{value}/{max}";
        _depthText.text = result;
    }

    private void SetVertexNodeColorBasedOnNodeColor(ENodeColor color)
    {
        var vertexColor = Color.white;
        switch (color)
        {
            case ENodeColor.WHITE:
                vertexColor = Color.white;
                break;
            case ENodeColor.GRAY:
                vertexColor = Color.gray;
                break;
            case ENodeColor.BLACK:
                vertexColor = Color.green;
                break;
            default:
                break;
        }

        SetVertexNodeColor(vertexColor);
    }

    public void SetCoord(Vector2Int coord)
    {
        this.coord = coord;

        this.transform.position = Level.CalculatePosition(coord);

        SetCoordText($"{coord.x} , {coord.y}");
    }

    private void SetCoordText(string value)
    {
        this.value = value;
        _coordText.text = value;
    }

    private void SetVertexNodeColor(Color color)
    {
        _vertexNode.color = color;
    }

    public void AddNeightbour(Vector3 position)
    {
        neightbours.Add(position);
    }

    public void CreateEdges()
    {
        ClearEdges();

        foreach (var neightbour in neightbours)
        {
            Vector2 direction = neightbour - (Vector2)transform.position;
            float angle = Util.GetAngleFromVectorFloat(direction);
            float distance = Vector2.Distance(transform.position, neightbour);
            var edge = Instantiate<GameObject>(edgePrefab, Vector3.zero, Quaternion.Euler(0, 0, angle), edgeParent);
            edge.transform.localPosition = Vector3.zero;
            edge.GetComponent<RectTransform>().sizeDelta = new Vector2(distance * 10, edgeSize);
            edge.GetComponentInChildren<Image>().color = lineColor;
            edges.Add(edge);
        }
    }

    private void ClearEdges()
    {
        foreach (var edge in edges)
        {
            DestroyImmediate(edge.gameObject);
        }

        edges.Clear();
    }

    private void OnDestroy()
    {
        UnSubscribeGraphManager(_graphManager);
        _graphManager.OnAlgorithmChanged -= OnAlgorithmChanged;
    }
}
