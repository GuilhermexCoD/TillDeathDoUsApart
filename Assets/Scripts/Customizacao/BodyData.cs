using UnityEngine;

[CreateAssetMenu(fileName = "New Body Part", menuName = "Customization/Part")]

public class BodyData : ScriptableObject
{
    public int id;

    public new string name;

    public Sprite visual;

    public Color color = Color.white;

    public EBodyPartType bodyPartType;
}
