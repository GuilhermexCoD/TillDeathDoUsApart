using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Util
{
    public const int sortingOrderDefault = 5000;

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

    public static string RemoveSpecialCharacters(string str)
    {
        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
    }

    public static float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (n < 0) n += 360;

        return n;
    }

    // Create a Text Popup in the World, no parent
    public static void CreateWorldTextPopup(string text, Vector3 localPosition, int fontSize, Vector3 localScale, float popupTime = 1f)
    {
        CreateWorldTextPopup(null, text, localPosition, fontSize, localScale, Color.white, localPosition + new Vector3(0, 20), popupTime);
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
}
