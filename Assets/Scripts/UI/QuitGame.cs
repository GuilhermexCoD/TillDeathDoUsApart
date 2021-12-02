using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    public static void Quit(bool menu)
    {
        if (!menu)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif

            Application.Quit();
        }
        else
        {
            SceneManager.LoadSceneAsync("Menu", LoadSceneMode.Single);
        }

    }
}
