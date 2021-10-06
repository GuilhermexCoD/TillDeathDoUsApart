using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitResult
{
    public bool blockingHit;
    public Vector3 traceStart;
    public Vector3 traceEnd;
    public Vector3 hitPosition;
    public Vector3 direction;
    public Vector3 normal;
    public Collider2D collider;
}
