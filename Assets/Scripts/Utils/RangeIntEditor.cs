using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
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

        serializedObject.ApplyModifiedProperties();
    }
}
#endif