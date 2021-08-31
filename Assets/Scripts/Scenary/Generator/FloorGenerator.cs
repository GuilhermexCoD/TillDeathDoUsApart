using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloorGenerator
{
    public static Dictionary<EWallTileType, HashSet<Vector2Int>> CreateFloors(HashSet<Vector2Int> floors)
    {
        var dictionary = new Dictionary<EWallTileType, HashSet<Vector2Int>>();

        UpdateFloors(ref dictionary, floors, Direction2D.CardinalDirections, FloorByteTypes.FloorCardinalDirections);

        return dictionary;
    }

    private static void UpdateFloors(ref Dictionary<EWallTileType, HashSet<Vector2Int>> dictionary, HashSet<Vector2Int> floors, Dictionary<EOrientation, Vector2Int> directions, Dictionary<EWallTileType, HashSet<int>> rules)
    {
        foreach (var floor in floors)
        {
            string neightboursBinaryType = string.Empty;
            foreach (var direction in directions)
            {
                var coordiante = floor + direction.Value;

                neightboursBinaryType += floors.Contains(coordiante) ? "1" : "0";
            }

            int typeAsInt = Convert.ToInt32(neightboursBinaryType, 2);
            EWallTileType type = EWallTileType.Full;

            foreach (var ruleType in rules)
            {
                if (ruleType.Value.Contains(typeAsInt))
                {
                    type = ruleType.Key;
                    break;
                }
            }
            bool success = dictionary.TryGetValue(type, out HashSet<Vector2Int> floorCoordinates);

            if (!success)
            {
                floorCoordinates = new HashSet<Vector2Int>();
                floorCoordinates.Add(floor);
                dictionary.Add(type, floorCoordinates);
            }
            else
            {
                floorCoordinates.Add(floor);
            }
        }
    }

    private static HashSet<Vector2Int> FindFloorDirections(HashSet<Vector2Int> floorCoordinates, Dictionary<EOrientation, Vector2Int> directions)
    {
        var wallsCoordinates = new HashSet<Vector2Int>();

        foreach (var floor in floorCoordinates)
        {
            foreach (var direction in directions)
            {
                var neighbourCoordinate = floor + direction.Value;
                if (!floorCoordinates.Contains(neighbourCoordinate))
                    wallsCoordinates.Add(neighbourCoordinate);
            }
        }

        return wallsCoordinates;
    }
}
