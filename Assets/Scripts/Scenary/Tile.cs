using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2Int coordinates;
    public ETheme theme;
    public GameObject Floor;
    [SerializeField]
    public List<Side> Sides;

    public Tile(Vector2Int coordinates,ETheme theme)
    {
        this.coordinates = coordinates;
        this.theme = theme;

        GenerateRandomTile();
    }

    public Tile(Vector2Int coordinates, GameObject floor,List<Side> sides)
    {
        this.coordinates = coordinates;
        Floor = floor;
        Sides = sides;
    }

    public void Setup(Vector2Int coordinates, ETheme theme)
    {
        this.coordinates = coordinates;
        this.theme = theme;

        GenerateRandomTile();
    }

    public Vector2Int GetCoordinates()
    {
        return coordinates;
    }

    public void GenerateRandomTile()
    {
        //TODO remove this class
    }

    private void Clean()
    {
        if (Floor != null)
            Destroy(Floor);

        Sides?.ForEach(side => { 
            if (side != null) 
            Destroy(side); 
        });
    }

    public override string ToString()
    {
        return $"{nameof(Vector2Int)} = {coordinates}";
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
