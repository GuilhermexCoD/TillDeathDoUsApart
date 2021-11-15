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
    private Vector2Int coord;

    private string value;

    [SerializeField]
    private GameObject edgePrefab;

    [SerializeField]
    private Color lineColor;
    [SerializeField]
    private float edgeSize = 10;
    [SerializeField]
    private Transform edgeParent;
    private List<Vector2> neightbours = new List<Vector2>();
    private List<GameObject> edges = new List<GameObject>();

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
            edge.GetComponent<RectTransform>().sizeDelta = new Vector2(distance*10, edgeSize);
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
