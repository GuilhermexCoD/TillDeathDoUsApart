using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Direction2D
{

    public static Dictionary<EOrientation, Vector2Int> CardinalDirections = new Dictionary<EOrientation, Vector2Int>
    {
        {EOrientation.Up, Vector2Int.up },
        {EOrientation.Right, Vector2Int.right },
        {EOrientation.Down, Vector2Int.down},
        {EOrientation.Left, Vector2Int.left }
    };

    public static Dictionary<EOrientation, Vector2Int> DiagonalDirections = new Dictionary<EOrientation, Vector2Int>
    {
        {EOrientation.UpRight, new Vector2Int(1, 1) },
        {EOrientation.DownRight, new Vector2Int(1, -1) },
        {EOrientation.DownLeft, new Vector2Int(-1, -1) },
        {EOrientation.UpLeft, new Vector2Int(-1, 1) }
    };

    public static Dictionary<EOrientation, Vector2Int> EightDirections = new Dictionary<EOrientation, Vector2Int>
    {
        {EOrientation.Up, Vector2Int.up },
        {EOrientation.UpRight, new Vector2Int(1, 1) },
        {EOrientation.Right, Vector2Int.right },
        {EOrientation.DownRight, new Vector2Int(1, -1) },
        {EOrientation.Down, Vector2Int.down},
        {EOrientation.DownLeft, new Vector2Int(-1, -1) },
        {EOrientation.Left, Vector2Int.left },
        {EOrientation.UpLeft, new Vector2Int(-1, 1) }
    };

    public static EOrientation GetRandomOrientation()
    {
        int max = CardinalDirections.Count;
        return (EOrientation)Random.Range(0, max);
    }

    public static Vector2Int GetRandomDirection()
    {
        EOrientation orientation = GetRandomOrientation();

        Vector2Int direction = Vector2Int.up;

        CardinalDirections.TryGetValue(orientation, out direction);

        return direction;
    }
}
