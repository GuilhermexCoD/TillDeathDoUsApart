using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ScenaryManager : MonoBehaviour
{
    public static ScenaryManager current;

    private int dungeonLevel = 1;
    private LevelData levelData;

    [SerializeField]
    private string seed;

    public event EventHandler<LevelArgs> onLevelLoaded;

    public void Subscribe(EventHandler<LevelArgs> handler)
    {
        onLevelLoaded += handler;
    }

    #region Setters

    public void SetSeed(string seed)
    {
        this.seed = seed;
    }

    public void SetLevelData(LevelData data)
    {
        this.levelData = data;
    }

    #endregion

    public void ResetDungeonLevel()
    {
        dungeonLevel = 1;
    }

    public void IncreaseDugeonLevel()
    {
        dungeonLevel++;
    }

    public static List<string> directionPath = new List<string>()
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
        "Center",
        "UpDown",
        "RightLeft",
        "UUp",
        "URight",
        "UDown",
        "ULeft",
    };

    public static Dictionary<ETileType, string> tilepath = new Dictionary<ETileType, string>
    {
        { ETileType.Floor, "Floors" },
        { ETileType.Wall, "Walls" }
    };

    public static Dictionary<string, List<TileBase>> assets = new Dictionary<string, List<TileBase>>();

    public static void LoadAssets()
    {
        foreach (var theme in Enum.GetValues(typeof(ETheme)))
        {
            foreach (var tile in tilepath)
            {
                foreach (var direction in directionPath)
                {
                    string path = GetPath((ETheme)theme, tile.Key, direction);

                    List<TileBase> tileAssets = GetResources(path);

                    if (tileAssets.Count > 0)
                    {
                        assets.Add(path, tileAssets);
                    }
                }
            }
        }
    }

    public static string GetPath(ETheme theme, ETileType tileType, string orientationPath)
    {
        string themePath = theme.ToString();

        tilepath.TryGetValue(tileType, out string tile);

        return $"{tile}/{themePath}/{orientationPath}";
    }

    public static List<TileBase> GetAssets(ETheme theme, ETileType tileType, string orientationPath)
    {
        List<TileBase> objects = new List<TileBase>();

        string path = GetPath(theme, tileType, orientationPath);

        var success = assets.TryGetValue(path, out objects);

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
        return assets.Count > 0;
    }

    public static void ClearAssets()
    {
        assets.Clear();
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (current != null)
        {
            Destroy(this.gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        current = Singleton<ScenaryManager>.Instance;
        LoadAssets();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex > 0)
        {
            Debug.LogWarning(seed.GetHashCode());
            Random.InitState(seed.GetHashCode());
        }

        onLevelLoaded?.Invoke(this, new LevelArgs()
        {
            levelCount = dungeonLevel,
            data = levelData
        });
    }
}
