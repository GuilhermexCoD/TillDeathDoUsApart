using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/SimpleItem")]
public class ItemData : ScriptableObject
{
    public int Id;
    public string Name;
    public Texture2D Icon;
    public Sprite Asset;
    public int Value;
}
