using System;
using UnityEngine;

public class OnShootEventArgs : EventArgs
{
    public int projectileIndex;
    public Vector3 position;
    public Vector3 direction;
    public float range;
    public float damage;
}