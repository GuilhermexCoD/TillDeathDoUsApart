using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenaryEvents : MonoBehaviour
{
    public static ScenaryEvents current;

    // Start is called before the first frame update
    void Awake()
    {
        current = Singleton<ScenaryEvents>.Instance;
    }

    public event Action<Tile> onTileCreation;

    public void TileCreated(Tile tile)
    {
        if (onTileCreation != null)
            onTileCreation(tile);
    }

    public event Action<Vector2Int> onCoordinateCreation;

    public void CoordinatesCreated(Vector2Int coord)
    {
        if (onCoordinateCreation != null)
            onCoordinateCreation(coord);
    }
}
