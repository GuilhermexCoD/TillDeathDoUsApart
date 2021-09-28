using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
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

        var inventoryManager = playerController.GetInventory();
        var items = inventoryManager.GetItems();
        foreach (var item in items)
        {
            int index = items.IndexOf(item);

            GUILayout.BeginHorizontal();
            GUILayout.Label($"{index}: {item}");

            GUILayout.EndHorizontal();
        }
        
    }
}
#endif