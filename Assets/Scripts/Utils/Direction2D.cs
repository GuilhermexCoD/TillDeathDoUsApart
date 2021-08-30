using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Direction2D
{

    public static Dictionary<EOrientation, Vector2Int> directions = new Dictionary<EOrientation, Vector2Int>
    {
        {EOrientation.Up, Vector2Int.up },
        {EOrientation.Right, Vector2Int.right },
        {EOrientation.Down, Vector2Int.down},
        {EOrientation.Left, Vector2Int.left },
    };

    public static EOrientation GetRandomOrientation()
    {
        int max = Enum.GetValues(typeof(EOrientation)).Length;
        return (EOrientation)Random.Range(0, max);
    }

    public static Vector2Int GetRandomDirection()
    {
        EOrientation orientation = GetRandomOrientation();

        Vector2Int direction = Vector2Int.up;

        directions.TryGetValue(orientation, out direction);

        return direction;
    }
}
