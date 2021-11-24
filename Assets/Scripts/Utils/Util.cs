using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Util
{
    public const int sortingOrderDefault = 5000;

    public static bool PythagoreanTheorem(float hypotenuse, float leg1, float leg2)
    {
        float pLeg1 = Mathf.Pow(leg1, 2);
        float pLeg2 = Mathf.Pow(leg2, 2);

        float pHypo = Mathf.Pow(hypotenuse, 2);

        return (pHypo == (pLeg1 + pLeg2));
    }

    public static float GetHypotenuse(float leg1,float leg2)
    {
        float pLeg1 = Mathf.Pow(leg1, 2);
        float pLeg2 = Mathf.Pow(leg2, 2);
        float sum = pLeg1 + pLeg2;

        float hypotenuse = Mathf.Sqrt(sum);

        return hypotenuse;
    }

    public static Vector3 GetMouseWorldPosition(Vector2 pointerPosition)
    {
        Vector3 vector = Vector3.zero;
        if (Camera.current != null)
        {
            //Vector3 vector = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.current);
            vector = GetMouseWorldPositionWithZ(pointerPosition, Camera.current);
            vector.z = 0;
        }
        
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

    public static int GetActualPage(int index, int clusterSize)
    {
        return Math.Abs(index / clusterSize) + 1;
    }

    public static float ModLoop(float value, float mod)
    {
        if (mod > 0)
        {
            return (value % mod + mod) % mod;
        }
        return 0;
    }

    public static int ModLoop(int value, int mod)
    {
        if (mod > 0)
        {
            return (value % mod + mod) % mod;
        }
        return 0;
    }

    public static string RemoveSpecialCharacters(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }

    public static float GetAngleFromVectorFloat(Vector2 direction)
    {
        direction = direction.normalized;

        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (n < 0) n += 360;

        return n;
    }

    public static float GetAngleFromVectorFloat(Vector2Int direction)
    {
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (n < 0) n += 360;

        return n;
    }

    public static float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (n < 0) n += 360;

        return n;
    }

    // Create Text in the World
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    // Create a Text Popup in the World
    public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Vector3 localScale, Color color, Vector3 finalPopupPosition, float popupTime)
    {
        TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left, sortingOrderDefault);
        Transform transform = textMesh.transform;
        transform.localScale = localScale;
        Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
        FunctionUpdater.Create(delegate ()
        {
            transform.position += moveAmount * Time.unscaledDeltaTime;
            popupTime -= Time.unscaledDeltaTime;
            if (popupTime <= 0f)
            {
                UnityEngine.Object.Destroy(transform.gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }, "WorldTextPopup");
    }

    // Create a Text Popup in the World, no parent
    public static void CreateWorldTextPopup(string text, Vector3 localPosition, int fontSize, Vector3 localScale, float speed, float popupTime = 1f)
    {
        CreateWorldTextPopup(null, text, localPosition, fontSize, localScale, Color.white, localPosition + new Vector3(0, speed), popupTime);
    }

    public static void CreateWorldTextPopup(string text, Vector3 localPosition, int fontSize, Vector3 localScale, Color color, float speed, float popupTime = 1f)
    {
        CreateWorldTextPopup(null, text, localPosition, fontSize, localScale, color, localPosition + new Vector3(0, speed), popupTime);
    }
}
