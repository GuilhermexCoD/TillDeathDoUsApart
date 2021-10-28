using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapVisualizer))]
public class Level : MonoBehaviour
{
    #region Debug Vars

    public Tilemap bspVisualizer;
    public TileBase debugTile;
    public int iteration = 0;

    #endregion

    public static float offSet = 0.5f;
    public static Level current;

    [SerializeField]
    private LevelData data;

    public List<BoundsInt> bounds = new List<BoundsInt>();

    public HashSet<Vector2Int> map = new HashSet<Vector2Int>();
    public List<Room<GeneratorRule>> rooms = new List<Room<GeneratorRule>>();
    public List<Color> roomColors = new List<Color>();
    public HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;

    public event EventHandler<EventArgs> onGenerated;

    private bool isLevelGenerated = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (!tilemapVisualizer)
        {
            tilemapVisualizer = this.GetComponent<TilemapVisualizer>();
        }

        current = Singleton<Level>.Instance;

        ScenaryManager.current?.Subscribe(OnLevelLoaded);

    }

    private void OnLevelLoaded(object sender, LevelArgs e)
    {
        data = e.data;
        Setup();
    }

    public void Setup()
    {
        Clean();

        Generate();

        tilemapVisualizer.Setup();

        tilemapVisualizer.PaintFloors(map);
        tilemapVisualizer.PaintWalls(map);

        CallOnGenerated();
    }

    public void Clean()
    {
        if (map != null)
        {
            map = new HashSet<Vector2Int>();
            rooms = new List<Room<GeneratorRule>>();
            roomColors = new List<Color>();
            corridors = new HashSet<Vector2Int>();
            tilemapVisualizer?.Clear();
        }
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

        //BinarySpacePartitioning
        if (data.useBSP)
        {

            var roomsCoordinates = BinarySpacePartitioningGenerator.BinarySpacePartitioning(new BoundsInt((Vector3Int)data.Start, (Vector3Int)data.size), data.roomMaxSize.x, data.roomMaxSize.y);

            foreach (var coord in roomsCoordinates)
            {
                if (rooms.Count() < data.maxQuantityOfRooms)
                {
                    RoomGeneration((Vector2Int)coord.min, ETheme.Castle, coord.size.x, coord.size.y);
                }
            }
        }
        else
        {
            for (int i = 0; i < data.maxQuantityOfRooms; i++)
            {
                RoomGeneration(GetRandomCoordinate(), ETheme.Castle, data.roomMaxSize.x, data.roomMaxSize.y);
            }
        }

        GenerateCorridors();
    }

    private void OnBinary(BoundsInt bound)
    {
        print("BINARY");
        bounds.Add(bound);
    }

    private void GenerateCorridors()
    {
        var indexes = new List<int>();

        for (int i = 0; i < rooms.Count; i++)
        {
            indexes.Add(i);
        }

        Room<GeneratorRule>[] auxRooms = new Room<GeneratorRule>[rooms.Count];

        rooms.CopyTo(auxRooms);

        var auxList = auxRooms.ToList();

        for (int i = 0; i < rooms.Count; i++)
        {
            var room = rooms[i];

            var roomOther = GetClosestRooms(room, auxList);

            var coords = CorridorsGenerator.ConnectCoordinates(room.GetCenterCoord(), roomOther.GetCenterCoord(), Random.value > 0.5f);

            corridors.UnionWith(coords);
            map.UnionWith(coords);

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

                roomColors.Add(Random.ColorHSV());

                break;
            case 1:
                List<EWalkType> walkTypes = new List<EWalkType>();
                walkTypes.Add(EWalkType.Random);
                generator = new RandomWalkGenerator(start, theme, size, walkTypes);

                roomColors.Add(Color.cyan);

                break;
            default:
                break;
        }

        var createdRoom = CreateRoom(start, theme, size, generator);

        rooms.Add(createdRoom);
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
            map.UnionWith(roomMap);
        }
    }

    public Vector2Int GetRandomCoordinate()
    {
        return new Vector2Int(Random.Range(0, data.size.x), Random.Range(0, data.size.y));
    }

    public static Vector3 CalculatePosition(Vector2Int coord)
    {
        Vector3 position = Vector3.zero;

        position += new Vector3(coord.x + offSet, coord.y + offSet);

        return position;
    }

    private void OnDestroy()
    {
        ScenaryManager.current?.UnSubscribe(OnLevelLoaded);
    }

    public Vector2Int GetLevelSize()
    {
        return data.size;
    }

    public Vector2Int GetRandomPositionInsideRoom()
    {
        return rooms[Random.Range(0, rooms.Count)].GetRandomCoord();
    }

    public void CallOnGenerated()
    {
        isLevelGenerated = true;
        onGenerated?.Invoke(this, new EventArgs());
    }

    public bool IsGenerated()
    {
        return isLevelGenerated;
    }

    #region Debug

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
        bspVisualizer.ClearAllTiles();
        if (iteration >= 0 && iteration < bounds.Count())
        {
            PrintDebugBound(bounds[iteration]);
        }
    }

    private void PrintDebugBound(BoundsInt bound)
    {
        for (int x = bound.min.x; x < bound.max.x; x++)
        {
            for (int y = bound.min.y; y < bound.max.y; y++)
            {
                var pos = new Vector2Int(x, y);
                PrintDebugTile(pos);
            }
        }
    }

    private void PrintDebugTile(Vector2Int coordinate)
    {
        TilemapVisualizer.PaintSingleTile(bspVisualizer, debugTile, coordinate);
    }

    public void CleanDebug()
    {
        bspVisualizer.ClearAllTiles();
        bounds.Clear();
    }

    #endregion
}
