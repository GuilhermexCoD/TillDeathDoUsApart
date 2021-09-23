using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationMenu : MonoBehaviour
{
    public PlayerVisualManager visualManager;

    public List<BodyData> headParts = new List<BodyData>();

    public int id = 0;

    public void Increase(int incremento)
    {
        id = (int)Util.ModLoop(id + incremento, headParts.Count);

        Debug.Log($"Incremento : {id}");
    }

    public void SetId(int id)
    {
        var part = headParts[id];

        visualManager.SetPart(EBodyPartType.Head, part);
        visualManager.UpdatePart(EBodyPartType.Head);
    }
}
