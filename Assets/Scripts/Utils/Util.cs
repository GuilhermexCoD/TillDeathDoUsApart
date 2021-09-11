using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.current);
        vector.z = 0;
        return vector;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static float MapRange(float value, float a1, float a2, float b1, float b2)
    {
        float result = 0;

        result = b1 + (value - a1) * (b2 - b1) / (a2 - a1);

        return result;
    }

    public static float ModLoop(float value, float mod)
    {
        return (value % mod + mod) % mod;
    }
}
