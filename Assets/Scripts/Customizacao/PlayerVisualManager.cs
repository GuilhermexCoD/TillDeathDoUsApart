using System;
using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
    public CustomizationData customizationData;
    public BodyData head;
    [SerializeField]
    public SpriteRenderer headSprite;

    public BodyData torso;
    [SerializeField]
    public SpriteRenderer torsoSprite;

    public BodyData handR;
    [SerializeField]
    public SpriteRenderer handR_Sprite;

    public BodyData handL;
    [SerializeField]
    public SpriteRenderer handL_Sprite;

    public BodyData footR;
    [SerializeField]
    public SpriteRenderer footR_Sprite;

    public BodyData footL;
    [SerializeField]
    public SpriteRenderer footL_Sprite;

    private void Awake()
    {
        UpdateVisualCustomizationData();
    }

    public void UpdateVisualCustomizationData()
    {
        head = customizationData.headData;
        torso = customizationData.torsoData;
        handL = customizationData.handData;
        handR = customizationData.handData;
        footL = customizationData.footData;
        footR = customizationData.footData;

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        for (int i = 0; i < Enum.GetValues(typeof(EBodyPartType)).Length; i++)
        {
            UpdatePart((EBodyPartType)i);
        }
    }

    public void SetPart(EBodyPartType bodyPartType, BodyData data)
    {
        switch (bodyPartType)
        {
            case EBodyPartType.Head:
                head = data;
                break;
            case EBodyPartType.Torso:
                torso = data;
                break;
            case EBodyPartType.Hand:
                handL = data;
                handR = data;
                break;
            case EBodyPartType.Foot:
                footL = data;
                footR = data;
                break;
            default:
                break;
        }
    }

    public void UpdatePart(EBodyPartType bodyPartType)
    {
        switch (bodyPartType)
        {
            case EBodyPartType.Head:
                UpdatePartVisual(head, headSprite);
                break;
            case EBodyPartType.Torso:
                UpdatePartVisual(torso, torsoSprite);
                break;
            case EBodyPartType.Hand:
                UpdatePartVisual(handR, handR_Sprite);
                UpdatePartVisual(handL, handL_Sprite);
                break;
            case EBodyPartType.Foot:
                UpdatePartVisual(footR, footR_Sprite);
                UpdatePartVisual(footL, footL_Sprite);
                break;
            default:
                break;
        }
    }

    private void UpdatePartVisual(BodyData data, SpriteRenderer spriteRenderer)
    {
        spriteRenderer.sprite = data.visual;
        spriteRenderer.color = data.color;
    }
}
