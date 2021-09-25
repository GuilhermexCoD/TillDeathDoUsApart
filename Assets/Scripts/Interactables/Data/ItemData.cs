using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/SimpleItem")]
public class ItemData : ScriptableObject
{
    [Header("Item Data")]

    public int id;
    public new string name;
    public Texture2D icon;
    public Sprite spriteAsset;
    public int value;
    public bool stackable = true;

    public override string ToString()
    {
        return ($"ID: {id} - Name: {name} - Value: {value}");
    }
}
