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
    private ETheme theme;

    public TilemapVisualizer(Tilemap tilemap)
    {
        this.LevelTilemap = tilemap;
    }

    public void Setup()
    {
        ScenaryManager.ClearAssets();

        ScenaryManager.LoadAssets();
    }

    public void PaintTiles(IEnumerable<Vector2Int> positions, ETileType type, EOrientation orientation)
    {
        foreach (var position in positions)
        {
            var tileBase = ScenaryManager.GetRandomAsset(theme, type, orientation);
            PaintSingleTile(LevelTilemap, tileBase, position);
        }
    }

    public void PaintTiles(IEnumerable<Vector2Int> positions, TileBase tileBase)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(LevelTilemap, tileBase, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        if (tilemap)
        {
            var tilePosition = tilemap.WorldToCell((Vector3Int)position);

            tilemap.SetTile(tilePosition, tile);
        }
        else
        {
            Debug.LogWarning("Nenhum asset encontrado");
        }

    }

    public void Clear()
    {
        if (LevelTilemap)
        {
            LevelTilemap.ClearAllTiles();
        }
    }
}
