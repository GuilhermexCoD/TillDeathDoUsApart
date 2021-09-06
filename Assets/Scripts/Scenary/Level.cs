using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapVisualizer))]
public class Level : MonoBehaviour
{
    public static Level Current;

    public Vector2Int Start;
    public int MaxQuantityOfRooms = 10;
    public EDifficulty Difficulty = EDifficulty.Easy;
    public int MaxQuantityOfEnemies = 20;

    public bool UseBSP = true;
    public Tilemap BSP_Visualizer;
    public TileBase DebugTile;
    public int iteration = 0;
    public List<BoundsInt> bounds = new List<BoundsInt>();

    public Vector2Int RoomMaxSize;
    public RangeInteger QuantityRangeBonusRooms;

    public int Width;
    public int Height;

    public HashSet<Vector2Int> Map = new HashSet<Vector2Int>();
    public List<Room<GeneratorRule>> Rooms = new List<Room<GeneratorRule>>();
    public List<Color> RoomColors = new List<Color>();
    public HashSet<Vector2Int> Corridors = new HashSet<Vector2Int>();

    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;

    // Start is called before the first frame update
    void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        if (!tilemapVisualizer)
        {
            tilemapVisualizer = this.GetComponent<TilemapVisualizer>();
        }

        Current = Singleton<Level>.Instance;

        Generate();

        tilemapVisualizer.Setup();

        tilemapVisualizer.PaintFloors(Map);
        tilemapVisualizer.PaintWalls(Map);

    }

    public void Clean()
    {
        if (Map != null)
        {
            Map = new HashSet<Vector2Int>();
            Rooms = new List<Room<GeneratorRule>>();
            RoomColors = new List<Color>();
            Corridors = new HashSet<Vector2Int>();
            tilemapVisualizer.Clear();
        }
    }

    public void CleanDebug()
    {
        BSP_Visualizer.ClearAllTiles();
        bounds.Clear();
    }

    public void Subscribe()
    {
        BinarySpacePartitioningGenerator.Subscribe(OnBinary);
    }
    public void UnSubscribe()
    {
        BinarySpacePartitioningGenerator.Unsubscribe(OnBinary);
    }

    public void Generate()
    {
        Clean();

        if (UseBSP)
        {

            var roomsCoordinates = BinarySpacePartitioningGenerator.BinarySpacePartitioning(new BoundsInt((Vector3Int)Start, new Vector3Int(Width, Height, 0)), RoomMaxSize.x, RoomMaxSize.y);

            //PrintDebugBoundsInt(roomsCoordinates);

            foreach (var coord in roomsCoordinates)
            {
                RoomGeneration((Vector2Int)coord.min, ETheme.Castle, coord.size.x, coord.size.y);
            }
        }
        else
        {
            for (int i = 0; i < MaxQuantityOfRooms; i++)
            {
                RoomGeneration(GetRandomCoordinate(), ETheme.Castle, RoomMaxSize.x, RoomMaxSize.y);
            }
        }

        GenerateCorridors();
    }

    private void OnBinary(BoundsInt bound)
    {
        print("BINARY");
        bounds.Add(bound);
    }

    public void IncreaseIteration()
    {
        iteration = Mathf.Clamp(iteration + 1, 0, bounds.Count());
    }

    public void DecreaseIteration()
    {
        iteration = Mathf.Clamp(iteration - 1, 0, bounds.Count());
    }

    public void PrintIteration()
    {
        BSP_Visualizer.ClearAllTiles();
        if (iteration >= 0 && iteration < bounds.Count())
        {
            PrintDebugBound(bounds[iteration]);
        }
    }

    private void PrintDebugBoundsInt(List<BoundsInt> bounds)
    {
        foreach (var bound in bounds)
        {
            PrintDebugBound(bound);
        }
    }

    private void PrintDebugBound(BoundsInt bound)
    {
        for (int x = bound.min.x; x < bound.max.x; x++)
        {
            for (int y = bound.min.y; y < bound.max.y; y++)
            {
                var pos = new Vector2Int(x, y);
                PrintDebugTile((Vector2Int)pos);
            }
        }
    }

    private void PrintDebugTile(Vector2Int coordinate)
    {
        TilemapVisualizer.PaintSingleTile(BSP_Visualizer, DebugTile, coordinate);
    }

    private void GenerateCorridors()
    {
        var indexes = new List<int>();

        for (int i = 0; i < Rooms.Count; i++)
        {
            indexes.Add(i);
        }

        Room<GeneratorRule>[] auxRooms = new Room<GeneratorRule>[Rooms.Count];

        Rooms.CopyTo(auxRooms);

        var auxList = auxRooms.ToList();

        for (int i = 0; i < Rooms.Count; i++)
        {
            var room = Rooms[i];

            var roomOther = GetClosestRooms(room, auxList);

            var coords = CorridorsGenerator.ConnectCoordinates(room.GetCenterCoord(), roomOther.GetCenterCoord(), Random.value > 0.5f);

            Corridors.UnionWith(coords);
            Map.UnionWith(coords);

            auxList.Remove(room);
        }

    }

    private void RoomGraphToCorridors(Graph<Room<GeneratorRule>, Weight> graph)
    {
        foreach (var vertexEdges in graph.GetEdgeList())
        {
            int indexA = vertexEdges.Key;

            var roomA = graph.GetVertex(indexA).GetData();

            foreach (var edge in vertexEdges.Value)
            {
                int indexB = edge.GetVertexIndex();
                var roomB = graph.GetVertex(indexB).GetData();
            }
        }
    }

    private void RoomGeneration(Vector2Int start, ETheme theme, int maxWidth, int maxHeight)
    {
        int generatorType = Random.Range(0, 2);

        int roomWidth = Random.Range(2, maxWidth);
        int roomHeight = Random.Range(2, maxHeight);

        int size = roomWidth * roomHeight;

        GeneratorRule generator = new GeneratorRule(start, theme, size);

        switch (generatorType)
        {
            case 0:
                generator = new TraditionalRoomGenerator(start, theme, size, roomWidth, roomHeight);

                RoomColors.Add(Random.ColorHSV());

                break;
            case 1:
                List<EWalkType> walkTypes = new List<EWalkType>();
                walkTypes.Add(EWalkType.Random);
                generator = new RandomWalkGenerator(start, theme, size, walkTypes);

                RoomColors.Add(Color.cyan);

                break;
            default:
                break;
        }

        var createdRoom = CreateRoom(start, theme, size, generator);

        Rooms.Add(createdRoom);
    }

    public Room<TGenerator> GetClosestRooms<TGenerator>(Room<TGenerator> targetRoom, List<Room<TGenerator>> rooms) where TGenerator : GeneratorRule
    {
        Room<TGenerator> closestRoom = rooms.FirstOrDefault();

        Vector2Int centerRoom = targetRoom.GetCenterCoord();

        var dist = float.MaxValue;

        foreach (var room in rooms)
        {
            if (room != targetRoom)
            {
                float currentRoomDist = Vector2Int.Distance(centerRoom, room.GetCenterCoord());

                if (currentRoomDist < dist)
                {
                    dist = currentRoomDist;
                    closestRoom = room;
                }
            }
        }

        return closestRoom;
    }

    public Room<TGenerator> CreateRoom<TGenerator>(Vector2Int start, ETheme theme, int size, TGenerator generator) where TGenerator : GeneratorRule
    {
        var room = new Room<TGenerator>(start, theme, size, this.transform, generator);

        room.Subscribe(OnRoomGenerated);

        room.Generate();

        return room;
    }

    public void OnRoomGenerated(HashSet<Vector2Int> roomMap)
    {
        if (roomMap != null && roomMap.Count > 0)
        {
            Map.UnionWith(roomMap);
        }
    }

    public Vector2Int GetRandomCoordinate()
    {
        return new Vector2Int(Random.Range(0, Width), Random.Range(0, Height));
    }

    public bool IsValidTileCoordinate(Vector2Int coord)
    {
        bool isInsideMap = coord.x >= 0 && coord.x < Width && (coord.y >= 0 && coord.y < Height);

        return isInsideMap;
    }

    private void DrawCoords(HashSet<Vector2Int> coords, Color color)
    {
        foreach (var coord in coords)
        {
            DrawCoord(coord, color);
        }
    }

    private void DrawCoord(Vector2Int coord, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawCube(CalculatePosition(coord, this.transform.position), new Vector3(1, 1, 0.1f));
    }

    private void DrawCorridors()
    {
        if (Corridors != null)
        {
            foreach (var coord in Corridors)
            {
                if (coord != null)
                {
                    DrawCoord(coord, Color.white);
                }
            }
        }
    }

    private void DrawRooms()
    {
        if (Rooms != null)
        {
            foreach (var room in Rooms)
            {
                int index = Rooms.IndexOf(room);
                if (room != null)
                {
                    DrawCoords(room.map, RoomColors[index]);
                }
            }
        }
    }

    private void DrawMap()
    {
        //DrawCorridors();
        //DrawRooms();
    }

    public static Vector3 CalculatePosition(Vector2Int coord, Vector3 startPosition)
    {
        Vector3 position = startPosition;

        position += new Vector3(coord.x, coord.y, 0);

        return position;
    }

    private void OnDrawGizmos()
    {
        DrawMap();
    }

    private void OnDestroy()
    {
        //TODO unsubscribe from Rooms 
    }
}
