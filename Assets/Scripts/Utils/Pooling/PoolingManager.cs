using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    public static PoolingManager current;

    public PoolingObject[] poolingObjects;

    private Dictionary<int, PoolingList> pooledDictionary;

    // Start is called before the first frame update
    void Awake()
    {
        current = Singleton<PoolingManager>.Instance;
        pooledDictionary = new Dictionary<int, PoolingList>();
    }

    // Update is called once per frame
    void Start()
    {
        foreach (var poolObj in poolingObjects)
        {
            int quantity = poolObj.initialQuantity;
            GameObject prefab = poolObj.pooledObject;
            Transform parent = poolObj.parent;
            PoolingList poolingList = new PoolingList();

            if (parent == null)
            {
                var parentGo = new GameObject($"{prefab.name}_poll");

                parentGo.transform.SetParent(this.transform);

                parent = parentGo.transform;

                poolObj.parent = parent;
            }

            for (int i = 0; i < quantity; i++)
            {
                GameObject poolGO = Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity, parent);
                poolingList.Add(poolGO);
                poolGO.SetActive(false);
            }

            pooledDictionary.Add(prefab.GetHashCode(), poolingList);
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        pooledDictionary.TryGetValue(prefab.GetHashCode(), out PoolingList poolingList);

        GameObject poolGo = poolingList?.GetPooledObject();

        if (poolGo != null)
        {
            poolGo.SetActive(true);
        }

        return poolGo;
    }

    public GameObject GetPooledObject(GameObject prefab, Transform parent)
    {
        GameObject poolGo = GetPooledObject(prefab);

        if (poolGo != null)
        {
            poolGo.transform.SetParent(parent, false);
        }

        return poolGo;
    }

    public void ReturnObjectToPoll(GameObject polledObject)
    {
        polledObject.SetActive(false);

        var poolingObject = poolingObjects.FirstOrDefault(p => p.Equals(polledObject));

        if (poolingObject != null)
        {
            polledObject.transform.SetParent(poolingObject.parent);
        }
    }
}
