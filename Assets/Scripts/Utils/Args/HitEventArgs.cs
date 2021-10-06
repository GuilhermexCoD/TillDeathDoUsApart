using System;
using UnityEngine;

public class HitEventArgs : EventArgs
{
    public int projectileIndex;
    public HitResult hitResult;
    public float damage;
}
