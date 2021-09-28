using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/SimpleItem")]
public class ItemData : ScriptableObject
{
    [Header("Item Data")]
    public new string name;
    public Texture2D icon;
    public Sprite spriteAsset;
    public int value;
    public bool stackable = true;
    public int quantity = 1;

    [Header("Anim transforms")]
    public Vector3 offSetPosition;
    public Vector3 handR_Transform;
    public Vector3 handR_Rotation;

    public Vector3 handL_Transform;
    public Vector3 handL_Rotation;

    private int id { get { return name.GetHashCode();}}

    public int GetId()
    {
        return id;
    }

    public override string ToString()
    {
        return ($"ID: {id} - Name: {name} - Value: {value}");
    }
}
