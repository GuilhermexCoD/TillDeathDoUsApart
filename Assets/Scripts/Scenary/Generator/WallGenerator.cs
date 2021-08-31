using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static Dictionary<EWallTileType, HashSet<Vector2Int>> CreateWalls(HashSet<Vector2Int> floors)
    {
        var dictionary = new Dictionary<EWallTileType, HashSet<Vector2Int>>();

        var basicWalls = FindWallsInDirections(floors, Direction2D.CardinalDirections);
        UpdateWalls(ref dictionary, basicWalls, floors, Direction2D.CardinalDirections, WallByteTypes.WallTypesCardinal);
        var cornerWalls = FindWallsInDirections(floors, Direction2D.DiagonalDirections);
        UpdateWalls(ref dictionary, cornerWalls, floors, Direction2D.EightDirections, WallByteTypes.WallTypesDiagonals);

        return dictionary;
    }

    private static void UpdateWalls(ref Dictionary<EWallTileType, HashSet<Vector2Int>> dictionary, HashSet<Vector2Int> walls, HashSet<Vector2Int> floors, Dictionary<EOrientation, Vector2Int> directions, Dictionary<EWallTileType, HashSet<int>> wallRules)
    {
        foreach (var wallCoordinate in walls)
        {
            string neightboursBinaryType = string.Empty;
            foreach (var direction in directions)
            {
                var coordiante = wallCoordinate + direction.Value;

                neightboursBinaryType += floors.Contains(coordiante) ? "1" : "0";
            }

            int typeAsInt = Convert.ToInt32(neightboursBinaryType, 2);
            EWallTileType? type = null;

            foreach (var wallType in wallRules)
            {
                if (wallType.Value.Contains(typeAsInt))
                {
                    type = wallType.Key;
                    break;
                }
            }
            if (type != null)
            {
                bool success = dictionary.TryGetValue(type.Value, out HashSet<Vector2Int> wallsCoordinates);

                if (!success)
                {
                    wallsCoordinates = new HashSet<Vector2Int>();
                    wallsCoordinates.Add(wallCoordinate);
                    dictionary.Add(type.Value, wallsCoordinates);
                }
                else
                {
                    wallsCoordinates.Add(wallCoordinate);
                }
            }
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorCoordinates, Dictionary<EOrientation, Vector2Int> directions)
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
