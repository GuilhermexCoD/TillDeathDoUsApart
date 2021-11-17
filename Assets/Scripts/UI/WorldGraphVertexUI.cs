using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldGraphVertexUI : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Vertex<Vector2Int> vertex;

    [SerializeField]
    private Vector2Int coord;

    private string value;

    [SerializeField]
    private GameObject edgePrefab;

    private int _lastValue;
    private int _total;

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

    public void OnInitialize(GraphManager graphManager, Vertex<Vector2Int> vertex)
    {
        graphManager.OnAlgorithmChanged += OnAlgorithmChanged;

        SetVertex(vertex);
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

        switch (algorithm)
        {
            case EGraphAlgorithm.bfs:
                graphManager.OnBFS_Executed += OnBFS_Executed;
                break;
            case EGraphAlgorithm.dfs:
                break;
            case EGraphAlgorithm.aStar:
                break;
            default:
                break;
        }
    }

    private void UnSubscribeGraphManager(GraphManager graphManager)
    {
        graphManager.OnBFS_Executed -= OnBFS_Executed;
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

        SetText(result);
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

        SetText($"{coord.x} , {coord.y}");
    }

    private void SetText(string value)
    {
        this.value = value;
        text.text = value;
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
}
