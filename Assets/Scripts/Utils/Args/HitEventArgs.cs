using System;
using UnityEngine;

public class HitEventArgs : EventArgs
{
    public int projectileIndex;
    public Vector3 startPosition;
    public Vector3 hitPosition;
    public Vector3 direction;
    public float range;
    public float damage;
    public Collider2D hitCollider;
}
