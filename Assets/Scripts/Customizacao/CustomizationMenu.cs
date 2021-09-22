using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationMenu : MonoBehaviour
{
    public PlayerVisualManager visualManager;

    public List<BodyData> headParts;

    public void SetId(int id)
    {
        var part = headParts[id];

        visualManager.SetPart(EBodyPartType.Head, part);
        visualManager.UpdatePart(EBodyPartType.Head);
    }
}
