using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(TrainingManager), true)]
[CanEditMultipleObjects]
public class TrainingManagerEditor : Editor
{
    List<TrainingManager> trainingManagers = new List<TrainingManager>();

    void OnEnable()
    {
        foreach (var tar in targets)
        {
            trainingManagers.Add((TrainingManager)tar);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate"))
        {
            foreach (var training in trainingManagers)
            {
                training.Setup();
            }
        }
    }
}
#endif
