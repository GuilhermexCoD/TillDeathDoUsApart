using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CustomizationData", menuName = "Customization/Data")]
public class CustomizationData : ScriptableObject
{
    public BodyData headData;
    public BodyData torsoData;
    public BodyData handData;
    public BodyData footData;
}
