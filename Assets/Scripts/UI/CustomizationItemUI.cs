using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CustomizationItemUI : MonoBehaviour
{
    public PlayerVisualManager visualManager;
    public EBodyPartType bodyPartType;
    public TextMeshProUGUI textBodyPart;
    public List<BodyData> parts;
    public int currentPart;

    private ResourceManager<EBodyPartType, BodyData> resourceManager;

    public event Action<int> onCurrentPartChanged;

    // Start is called before the first frame update
    void Awake()
    {
        resourceManager = new ResourceManager<EBodyPartType, BodyData>("Customization");

        parts = resourceManager.GetAssets((int)bodyPartType);
        currentPart = 0;
        UpdateText();
    }

    private void UpdateText()
    {
        if (textBodyPart != null)
            textBodyPart.text = bodyPartType.ToString();
    }

    public void IncreasePart(int value)
    {
        currentPart = Util.ModLoop(this.currentPart + value, parts.Count);
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        var part = parts[currentPart];
        //visualManager.SetPart(bodyPartType, part);
        //visualManager.UpdatePart(bodyPartType);
        var customization = ScenaryManager.current.GetCustomizationData();

        switch (bodyPartType)
        {
            case EBodyPartType.Head:
                customization.headData = part;
                break;
            case EBodyPartType.Torso:
                customization.torsoData = part;
                break;
            case EBodyPartType.Hand:
                customization.handData = part;
                break;
            case EBodyPartType.Foot:
                customization.footData = part;
                break;
            default:
                break;
        }

        visualManager.UpdateVisualCustomizationData();
    }
}
