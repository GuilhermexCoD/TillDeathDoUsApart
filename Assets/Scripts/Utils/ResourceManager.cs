using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using Object = UnityEngine.Object;

public class ResourceManager<E, AssetType> where E : Enum where AssetType : Object
{
    private string _path;

    private Dictionary<int, List<AssetType>> _assets;

    public ResourceManager(string path)
    {
        _path = path;

        _assets = new Dictionary<int, List<AssetType>>();

        Load();
    }

    public void Load()
    {
        foreach (var name in Enum.GetNames(typeof(E)))
        {
            int value = (int)Enum.Parse(typeof(E), name);
            string resourcePath = $"{_path}/{name}";

            var assets = new List<AssetType>(Resources.LoadAll<AssetType>(resourcePath));

            if (assets.Count > 0)
            {
                _assets.Add(value, assets);
            }
        }
    }

    public List<AssetType> GetAssets(int key)
    {
        _assets.TryGetValue(key, out List<AssetType> assets);

        return assets;
    }
}
