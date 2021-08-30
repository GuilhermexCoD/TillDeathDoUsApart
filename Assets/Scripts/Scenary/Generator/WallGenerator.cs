using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static Dictionary<EOrientation, HashSet<Vector2Int>> CreateWalls(HashSet<Vector2Int> floorCoordinates)
    {
        var dictionary = new Dictionary<EOrientation, HashSet<Vector2Int>>();

        foreach (var direction in Direction2D.directions)
        {
            bool success = dictionary.TryGetValue(direction.Key, out HashSet<Vector2Int> wallsCoordinates);

            if (!success)
                wallsCoordinates = new HashSet<Vector2Int>();

            foreach (var floor in floorCoordinates)
            {
                var neighbourCoordinate = floor + direction.Value;
                if (!floorCoordinates.Contains(neighbourCoordinate))
                    wallsCoordinates.Add(neighbourCoordinate);
            }

            if (!success && wallsCoordinates.Count > 0)
                dictionary.Add(direction.Key, wallsCoordinates);
        }

        return dictionary;
    }
}
