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

    //TODO : remover esse prefab do Graph
    public GameObject GraphVertexPrefab;

    #endregion

    public const string PREFAB_DATA_PATH = "Prefabs";

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
    public event Action onClear;

    private bool isLevelGenerated = false;

    [SerializeField]
    private GameObject _prefabExit;
    [SerializeField]
    private GameObject _prefabNodeManager;
    [SerializeField]
    private GameObject _prefabRoomOrder;

    private NodeManager _nodeManager;
    private GameObject _roomOrderVisuals;

    public Transform enemyParent;
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
            onClear?.Invoke();
        }

        if (_nodeManager != null)
            Destroy(_nodeManager.gameObject);

        if (_roomOrderVisuals != null)
            Destroy(_roomOrderVisuals.gameObject);

        WorldCounter.ResetCounter();
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

        //TODO Executar BFS e encontrar o room mais distante para colocar a saida
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

    public void EnableDebug(bool value)
    {
        if (_nodeManager != null)
            _nodeManager.gameObject.SetActive(value);

        if (_roomOrderVisuals != null)
            _roomOrderVisuals.SetActive(value);
    }

    public void GenerateExit(Vector2Int startCoord)
    {
        GameObject exitPrefab = Resources.LoadAll<PrefabData>(PREFAB_DATA_PATH).Where(p => p.name == "Exit").FirstOrDefault().prefab;

        GraphManager.current.SetGraphFromMap(map);

        var coords = GraphManager.current.ExecuteBFS(startCoord, 0.1f);

        var targetCoord = coords[Random.Range(0, coords.Count)];

        GraphManager.current.ExecuteAStar(startCoord, targetCoord);
        HashSet<int> roomsOnPath = new HashSet<int>();

        _nodeManager = Instantiate<GameObject>(_prefabNodeManager, null).GetComponent<NodeManager>();

        var aStarPathReverse = GraphManager.current.aStarPath;

        aStarPathReverse.Reverse();
        var exitPos = CalculatePosition(aStarPathReverse[0].GetData().GetVertexData());
        Instantiate(_prefabExit, exitPos, Quaternion.identity);
        List<Vector2> path = new List<Vector2>();

        foreach (var coord in aStarPathReverse)
        {
            var coordVector = coord.GetData().GetVertexData();
            path.Add(CalculatePosition(coordVector));

            int roomIndex = GetRoomWithCoord(coordVector);
            if (roomIndex != -1)
                roomsOnPath.Add(roomIndex);
        }

        _nodeManager.CreatePath(path, GameEventsHandler.current.playerGo.transform);
        var roomPathArray = roomsOnPath.ToArray().Reverse();
        _roomOrderVisuals = new GameObject("RoomOrder");

        int enemyCount = 2;
        int roomCount = 0;
        foreach (var roomIndex in roomPathArray)
        {
            var counter = Instantiate<GameObject>(_prefabRoomOrder, _roomOrderVisuals.transform);
            var room = rooms[roomIndex];

            var coord = room.GetCenterCoord();

            int totalEnemy = Mathf.Min(enemyCount * roomCount,data.maxQuantityOfEnemies);
            for (int i = 0; i < totalEnemy; i++)
            {
                var enemy = Instantiate<GameObject>(GameManager.current.enemyPrefab, enemyParent);
                enemy.transform.position = CalculatePosition(room.GetRandomCoord());
            }

            roomCount++;

            var pos = CalculatePosition(coord);

            counter.transform.position = pos;
        }

        EnableDebug(false);
    }

    public int GetRoomWithCoord(Vector2Int coord)
    {

        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].HasCoord(coord))
                return i;
        }

        return -1;
    }

    //private void RoomGraphToCorridors(Graph<Room<GeneratorRule>, Weight> graph)
    //{
    //    foreach (var vertexEdges in graph.GetEdgeList())
    //    {
    //        int indexA = vertexEdges.Key;

    //        var roomA = graph.GetVertex(indexA).GetData();

    //        foreach (var edge in vertexEdges.Value)
    //        {
    //            int indexB = edge.GetVertexIndex();
    //            var roomB = graph.GetVertex(indexB).GetData();
    //        }
    //    }
    //}

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

    public static Vector2Int PositionToCoord(Vector2 position)
    {
        return new Vector2Int(Mathf.RoundToInt(position.x - offSet), Mathf.RoundToInt(position.y - offSet));
    }

    public static Vector3 CalculatePosition(Vector2Int coord)
    {
        return new Vector3(coord.x + offSet, coord.y + offSet);
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
