using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingList : ICollection<GameObject>
{
    public List<GameObject> pooling;

    private int _index = 0;

    public PoolingList()
    {
        this.pooling = new List<GameObject>();
        _index = 0;
    }

    public PoolingList(List<GameObject> pooling)
    {
        this.pooling = pooling;
        _index = 0;
    }

    public GameObject GetPooledObject()
    {
        GameObject result = null;

        if (_index < pooling.Count)
        {
            result = pooling[_index];
            
            _index = (_index + 1) % pooling.Count;
        }

        return result;
    }

    public int Count => pooling.Count;

    public bool IsReadOnly => false;

    public void Add(GameObject item)
    {
        pooling.Add(item);
    }

    public void Clear()
    {
        pooling.Clear();
    }

    public bool Contains(GameObject item)
    {
        return pooling.Contains(item);
    }

    public void CopyTo(GameObject[] array, int arrayIndex)
    {
        pooling.CopyTo(array, arrayIndex);
    }

    public bool Remove(GameObject item)
    {
        return pooling.Remove(item);
    }

    public IEnumerator<GameObject> GetEnumerator()
    {
        return pooling.GetEnumerator();
    }

    System.Collections.IEnumerator IEnumerable.GetEnumerator()
    {
        return pooling.GetEnumerator();
    }
}
