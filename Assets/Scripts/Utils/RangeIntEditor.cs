using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
[CanEditMultipleObjects]
public class RangeIntEditor : Editor
{
    Level level;

    void OnEnable()
    {
        level = (Level)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        if (GUILayout.Button("Generate Value"))
        {
            level.QuantityRangeBonusRooms.GenerateValue();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
