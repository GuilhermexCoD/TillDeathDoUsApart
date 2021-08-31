using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private List<Tilemap> Tilemaps;

    [SerializeField]
    private ETheme Theme;

    public void Setup()
    {
        ScenaryManager.ClearAssets();

        ScenaryManager.LoadAssets();
    }

    public void PaintTiles(IEnumerable<Vector2Int> positions, ETileType tileType, string orientation)
    {
        foreach (var position in positions)
        {
            var tileBase = ScenaryManager.GetRandomAsset(Theme, tileType, orientation);
            PaintSingleTile(GetTilemap(tileType), tileBase, position);
        }
    }

    public void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tileBase)
    {
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tileBase, position);
        }
    }

    public Tilemap GetTilemap(ETileType tileType)
    {
        Tilemap tilemap = null;

        if (Tilemaps.Count > 0)
        {
            tilemap = Tilemaps[(int)tileType];
        }

        return tilemap;
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
        Tilemaps.ForEach(t => t.ClearAllTiles());
    }
}
