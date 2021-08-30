using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenaryManager : MonoBehaviour
{
    public static ScenaryManager current;

    public string seed;

    public const int FLOOR_SIZE = 10;
    public const string FLOOR_PATH = "Floors/";

    public List<string> paths;

    public Dictionary<string, List<GameObject>> assets = new Dictionary<string, List<GameObject>>();

    private void LoadAssets()
    {
        foreach (var path in paths)
        {
            List<GameObject> objects = GetResources(path);

            assets.Add(path,objects);

        }
        
    }

    public List<GameObject> GetAssets(string path)
    {
        List<GameObject> objects = new List<GameObject>();

        var success = assets.TryGetValue(path, out objects);

        return (success)?objects:null;
    }

    public void PrintList(List<GameObject> gameObjects)
    {
        gameObjects.ForEach(g => print(g.name));
    }

    public static List<GameObject> GetResources(string path)
    {
        return new List<GameObject>(Resources.LoadAll<GameObject>(path));
    }
    // Start is called before the first frame update
    void Awake()
    {
        current = Singleton<ScenaryManager>.Instance;
        print(seed.GetHashCode());
        Random.InitState(seed.GetHashCode());
        print($"Testing Random: {Random.Range(0,11)}");
        LoadAssets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
