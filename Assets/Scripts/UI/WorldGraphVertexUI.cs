using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldGraphVertexUI : MonoBehaviour
{
    public Room<GeneratorRule> room;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Vector2Int coord;

    private string value;

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
}
