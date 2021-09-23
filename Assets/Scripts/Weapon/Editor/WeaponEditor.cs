using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor
{
    Weapon weapon;

    private void Awake()
    {
        weapon = (Weapon)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Update Visuals"))
        {
            weapon.UpdateVisuals();
        }

        if (GUILayout.Button("Attack"))
        {
            weapon.Attack();
        }
    }
}

#endif