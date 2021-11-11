using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolingObject
{
    public int initialQuantity;
    public GameObject pooledObject;
    public Transform parent;

    public override bool Equals(object obj)
    {
        return pooledObject.GetHashCode() == obj.GetHashCode();
    }
}
