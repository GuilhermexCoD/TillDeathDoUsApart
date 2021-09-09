using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerController), true)]
public class PlayerControllerEditor : Editor
{
    PlayerController playerController;

    private void Awake()
    {
        playerController = (PlayerController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("Inventory");
        foreach (var item in playerController.inventory)
        {
            int index = playerController.inventory.IndexOf(item);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{index}: {item}");

            GUILayout.EndHorizontal();
        }
        
    }
}