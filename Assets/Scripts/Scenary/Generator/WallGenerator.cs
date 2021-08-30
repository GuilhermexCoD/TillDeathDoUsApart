using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static HashSet<Vector2Int> CreateWalls(HashSet<Vector2Int> floorCoordinates)
    {
        var wallsCoordinates = new HashSet<Vector2Int>();

        foreach (var floor in floorCoordinates)
        {
            foreach (var direction in Direction2D.directions)
            {
                var neighbourCoordinate = floor + direction.Value;
                if (!floorCoordinates.Contains(neighbourCoordinate))
                    wallsCoordinates.Add(neighbourCoordinate);
            }
        }

        return wallsCoordinates;
    }
}
