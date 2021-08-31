using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ScenaryManager : MonoBehaviour
{
    public static ScenaryManager Current;

    public string Seed;

    public static List<string> DirectionPath = new List<string>()
    {
        "Up",
        "Right",
        "Down",
        "Left",
        "InnerCornerDownRight",
        "InnerCornerDownLeft",
        "DiagonalCornerUpRight",
        "DiagonalCornerUpLeft",
        "DiagonalCornerDownRight",
        "DiagonalCornerDownLeft",
        "Full",
        "FullEightDirections",
        "BottomEightDirections",
        "Center"
    };

    public static Dictionary<ETileType, string> Tilepath = new Dictionary<ETileType, string>
    {
        { ETileType.Floor, "Floors" },
        { ETileType.Wall, "Walls" }
    };

    public static Dictionary<string, List<TileBase>> Assets = new Dictionary<string, List<TileBase>>();

    public static void LoadAssets()
    {
        foreach (var theme in Enum.GetValues(typeof(ETheme)))
        {
            foreach (var tile in Tilepath)
            {
                foreach (var direction in DirectionPath)
                {
                    string path = GetPath((ETheme)theme, tile.Key, direction);

                    List<TileBase> tileAssets = GetResources(path);

                    if (tileAssets.Count > 0)
                    {
                        Assets.Add(path, tileAssets);
                    }
                }
            }
        }
    }

    public static string GetPath(ETheme theme, ETileType tileType, string orientationPath)
    {
        string themePath = theme.ToString();

        Tilepath.TryGetValue(tileType, out string tile);

        return $"{tile}/{themePath}/{orientationPath}";
    }

    public static List<TileBase> GetAssets(ETheme theme, ETileType tileType, string orientationPath)
    {
        List<TileBase> objects = new List<TileBase>();

        string path = GetPath(theme, tileType, orientationPath);

        var success = Assets.TryGetValue(path, out objects);

        return (success) ? objects : null;
    }

    public static TileBase GetRandomAsset(ETheme theme, ETileType tileType, string orientationPath)
    {
        List<TileBase> objects = GetAssets(theme, tileType, orientationPath);

        if (objects != null)
        {
            int rnd = 0;

            rnd = Random.Range(0, objects.Count);

            TileBase tile = objects[rnd];

            return tile;
        }

        return null;
    }

    public static List<TileBase> GetResources(string path)
    {
        return new List<TileBase>(Resources.LoadAll<TileBase>(path));
    }

    public static bool HasLoadedAssets()
    {
        return Assets.Count > 0;
    }

    public static void ClearAssets()
    {
        Assets.Clear();
    }

    // Start is called before the first frame update
    void Awake()
    {
        Current = Singleton<ScenaryManager>.Instance;
        print(Seed.GetHashCode());
        Random.InitState(Seed.GetHashCode());
        print($"Testing Random: {Random.Range(0, 11)}");
        LoadAssets();
    }
}
