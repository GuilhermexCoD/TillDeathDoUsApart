using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(PlayerVisualManager), true)]
public class PlayerVisualManagerEditor : Editor
{
    PlayerVisualManager playerVisual;
    // Start is called before the first frame update
    void Awake()
    {
        playerVisual = (PlayerVisualManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Visual"))
        {
            playerVisual.UpdateVisuals();
        }
    }
}

#endif