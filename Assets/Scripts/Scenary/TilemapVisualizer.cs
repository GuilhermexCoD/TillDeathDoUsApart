using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap LevelTilemap;

    [SerializeField]
    private TileBase FloorTile;

    [SerializeField]
    private TileBase WallTile;


    public TilemapVisualizer(Tilemap tilemap)
    {
        this.LevelTilemap = tilemap;
    }

    public void PaintTiles(IEnumerable<Vector2Int> positions, ETileType type)
    {
        foreach (var position in positions)
        {
            var tileBase = GetTileBase(type);
            PaintSingleTile(LevelTilemap, tileBase, position);
        }
    }

    private TileBase GetTileBase(ETileType type)
    {
        var tileBase = FloorTile;
        switch (type)
        {
            case ETileType.Floor:
                tileBase = FloorTile;
                break;
            case ETileType.Wall:
                tileBase = WallTile;
                break;
            default:
                break;
        }
        return tileBase;
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);

        tilemap.SetTile(tilePosition, tile);
    }

    public void Clear()
    {
        if (LevelTilemap)
        {
            LevelTilemap.ClearAllTiles();
        }
    }
}
