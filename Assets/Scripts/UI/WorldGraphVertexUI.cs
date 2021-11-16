using System;
using System.Collections;
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

    private int _totalVertices;

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

    public void SetVertex(Vertex<Vector2Int> vertex, int totalVertices)
    {
        this.vertex = vertex;
        //this.vertex.onColorChanged += OnColorChanged;
        //this.vertex.onStartTimeChanged += OnStartTimeChanged;
        //this.vertex.onEndTimeChanged += OnEndTimeChanged;
        this.vertex.onDistanceChanged += OnDistanceChanged;

        //TODO tirar essa distancia Hardcoded
        this._totalVertices = 144;

        SetCoord(vertex.GetData());
    }

    private void OnDistanceChanged(int value)
    {
        UpdateColor(value);
        UpdateLabel(value, _totalVertices);
    }

    private float GetNormalizedTime(int value)
    {
        return (float)value / (float)_totalVertices;
    }

    private void UpdateColor(int value)
    {
        var color = gradient.Evaluate(GetNormalizedTime(value));

        SetVertexNodeColor(color);
    }

    public void UpdateLabel(int value, int max)
    {
        string result = $"{value}/{max}";

        SetText(result);
    }

    public void UpdateDFS_Label()
    {
        int startTime = vertex.GetStartTime();
        int endTime = vertex.GetEndTime();

        UpdateLabel(startTime, endTime);
    }

    private void OnEndTimeChanged(int value)
    {
        UpdateDFS_Label();
        UpdateColor(value);
    }

    private void OnStartTimeChanged(int value)
    {
        UpdateColor(value);
    }

    private void SetVertexNodeColor(Color color)
    {
        _vertexNode.color = color;
    }

    private void OnColorChanged(ENodeColor color)
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

        _vertexNode.color = vertexColor;
    }

    public void SetCoord(Vector2Int coord)
    {
        this.coord = coord;

        this.transform.position = Level.CalculatePosition(coord);

        //SetText($"{coord.x} , {coord.y}");
    }

    private void SetText(string value)
    {
        this.value = value;
        text.text = value;
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
