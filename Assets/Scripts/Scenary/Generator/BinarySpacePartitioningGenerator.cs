using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinarySpacePartitioningGenerator : GeneratorRule
{
    public BinarySpacePartitioningGenerator(Vector2Int start, ETheme theme, int size) : base(start, theme, size)
    {
    }

    public override void Generate()
    {
        base.Generate();
    }

    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();

        List<BoundsInt> roomsList = new List<BoundsInt>();

        roomsQueue.Enqueue(spaceToSplit);

        while (roomsQueue.Count > 0)
        {
            var room = roomsQueue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                bool horizontally = Random.value < 0.5f;

                bool predicateHorizontally = (room.size.y >= minHeight * 2);
                bool predicateVertically = (room.size.x >= minWidth * 2);

                if ((horizontally && predicateHorizontally) || (!horizontally && predicateVertically))
                {
                    Split((horizontally)? minHeight:minWidth, roomsQueue, room, horizontally);
                }
                else if ((horizontally && predicateVertically) || (!horizontally && predicateHorizontally))
                {
                    Split((!horizontally) ? minWidth:minHeight, roomsQueue, room, !horizontally);
                }
                else if (room.size.x >= minWidth && room.size.y >= minHeight)
                {
                    roomsList.Add(room);
                }
            }
        }
        return roomsList;
    }

    private static void Split(int min, Queue<BoundsInt> roomsQueue, BoundsInt room,bool horizontally)
    {
        var splitAmount = Random.Range(1, (horizontally)?room.size.y:room.size.x);

        var room1Max = (horizontally)? new Vector3Int(room.size.x, splitAmount, room.size.z): new Vector3Int(splitAmount, room.size.y, room.size.z);
        BoundsInt room1 = new BoundsInt(room.min, room1Max);

        var room2Min = (horizontally) ? new Vector3Int(room.min.x,room.min.y + splitAmount, room.min.z) : new Vector3Int(room.min.x + splitAmount, room.min.y, room.min.z);
        var room2Max = (horizontally) ? new Vector3Int(room.size.x, room.size.y - splitAmount, room.size.z) : new Vector3Int(room.size.x - splitAmount, room.size.y, room.size.z);
        BoundsInt room2 = new BoundsInt(room2Min, room2Max);

        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}
