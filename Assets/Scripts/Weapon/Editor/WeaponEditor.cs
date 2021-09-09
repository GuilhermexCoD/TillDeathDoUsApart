using UnityEditor;
using UnityEngine;

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
