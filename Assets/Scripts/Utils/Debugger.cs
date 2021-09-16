using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Debugger
{
    // Creates a Text pop up at the world position
    public static void TextPopup(string text, Vector3 position, int fontSize, Vector3 localScale, float popupTime = 1f)
    {
        Util.CreateWorldTextPopup(text, position, fontSize, localScale, popupTime);
    }
}
