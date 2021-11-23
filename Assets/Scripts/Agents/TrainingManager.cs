using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrainingManager : MonoBehaviour
{
    public LevelData levelData;

    public TilemapVisualizer tilemapVisualizer;

    public GameObject agent;
    public Vector3 agentStartPosition;

    public GameObject[] altars;

    public Vector2Int minSize;

    public Vector2Int startOffset;

    public Vector2Int offsetIndex;

    //public List<Vector3> agentPositions = new List<Vector3>();
    //public List<Vector3> agentVelocity = new List<Vector3>();

    #region Level

    public HashSet<Vector2Int> map = new HashSet<Vector2Int>();
    public List<Room<GeneratorRule>> rooms = new List<Room<GeneratorRule>>();
    public HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();

    public event EventHandler<EventArgs> onGenerated;

    private bool _isLevelGenerated = false;

    #endregion

    public bool IsLevelGenerated()
    {
        return _isLevelGenerated;
    }

    void Awake()
    {
        if (!tilemapVisualizer)
        {
            tilemapVisualizer = this.GetComponent<TilemapVisualizer>();
        }

        if (agent != null)
        {
            agent.GetComponent<MoveToGoalAgent>().onEndEpisode += TrainingManager_onEndEpisode;
        }

        Setup();
    }

    private void FixedUpdate()
    {
        //agentPositions.Add(agent.transform.localPosition);
        //agentVelocity.Add(agent.GetComponent<MoveToGoalAgent>()._move.GetVelocity());
        //if (Vector3.Distance(agent.transform.position,this.transform.position) > 40)
        //{
        //    Debug.LogError("PARAAAAAA");
        //    Time.timeScale = 0;
        //}
    }

    private void TrainingManager_onEndEpisode(object sender, EventArgs e)
    {
        Setup();
    }

    public void Setup()
    {
        levelData.Start = startOffset * offsetIndex;

        Clean();

        Generate();

        tilemapVisualizer.Setup();

        tilemapVisualizer.PaintFloors(map);
        tilemapVisualizer.PaintWalls(map);

        CallOnGenerated();
    }

    public void CallOnGenerated()
    {
        _isLevelGenerated = true;
        onGenerated?.Invoke(this, new EventArgs());

        agentStartPosition = GetRandomPositionInsideRoom();
        agent.GetComponent<MoveToGoalAgent>().SetPositionVelocityZero(agentStartPosition);

        Vector2Int agentPos = new Vector2Int(Mathf.FloorToInt(agent.transform.position.x), Mathf.FloorToInt(agent.transform.position.y));
        Debug.LogWarning($"SPAWNED Agent On Position: {agentPos}");

        if (!map.Contains(agentPos))
        {
            Debug.LogError("SPAWNED Agent On Invalid Position");
        }

        var agentVelocity = agent.GetComponent<MoveToGoalAgent>()._move.GetVelocity();
        if (agentVelocity.magnitude > 0.1f)
        {
            Debug.LogError("Agent Velocity Is Not Ideal");
        }
        //foreach (var altar in altars)
        //{
        //    randomAgentPosition = GetRandomPositionInsideRoom();
        //    altar.transform.position = randomAgentPosition;
        //}
    }

    public void Clean()
    {
        _isLevelGenerated = false;
        map?.Clear();
        rooms?.Clear();
        corridors?.Clear();
        tilemapVisualizer?.Clear();
        //agentPositions?.Clear();
        //agentVelocity?.Clear();
    }

    public void Generate()
    {

        //BinarySpacePartitioning
        if (levelData.useBSP)
        {

            var roomsCoordinates = BinarySpacePartitioningGenerator.
                BinarySpacePartitioning(new BoundsInt((Vector3Int)levelData.Start, (Vector3Int)levelData.size),
                                                                levelData.roomMaxSize.x, levelData.roomMaxSize.y);

            foreach (var coord in roomsCoordinates)
            {
                if (rooms.Count() < levelData.maxQuantityOfRooms)
                {
                    RoomGeneration((Vector2Int)coord.min, ETheme.Castle, coord.size.x, coord.size.y);
                }
            }
        }
        else
        {
            for (int i = 0; i < levelData.maxQuantityOfRooms; i++)
            {
                RoomGeneration(GetRandomCoordinate(), ETheme.Castle, levelData.roomMaxSize.x, levelData.roomMaxSize.y);
            }
        }

        GenerateCorridors();
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

    private void RoomGeneration(Vector2Int start, ETheme theme, int maxWidth, int maxHeight)
    {
        int generatorType = Random.Range(0, 2);

        int roomWidth = Random.Range(minSize.x, maxWidth);
        int roomHeight = Random.Range(minSize.y, maxHeight);

        int size = roomWidth * roomHeight;

        GeneratorRule generator = new GeneratorRule(start, theme, size);

        switch (generatorType)
        {
            case 0:
                generator = new TraditionalRoomGenerator(start, theme, size, roomWidth, roomHeight);

                break;
            case 1:
                List<EWalkType> walkTypes = new List<EWalkType>();
                walkTypes.Add(EWalkType.Random);
                generator = new RandomWalkGenerator(start, theme, size, walkTypes);

                break;
            default:
                break;
        }

        var createdRoom = CreateRoom(start, theme, size, generator);

        rooms.Add(createdRoom);
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
        return new Vector2Int(Random.Range(levelData.Start.x, levelData.Start.x + levelData.size.x), Random.Range(levelData.Start.y, levelData.Start.y + levelData.size.y));
    }

    public Vector3 GetRandomPositionInsideRoom()
    {
        var coord = rooms[Random.Range(0, rooms.Count)].GetRandomCoord();
        return new Vector3(coord.x, coord.y) + (Vector3.one * 0.5f);
    }

    public Vector3 CalculatePosition(Vector2Int coord)
    {
        Vector3 position = Vector3.zero;

        //var offSet = levelData.Start;
        var offSet = Vector3.zero;
        position += new Vector3(coord.x - offSet.x, coord.y - offSet.y);

        return position;
    }
}
