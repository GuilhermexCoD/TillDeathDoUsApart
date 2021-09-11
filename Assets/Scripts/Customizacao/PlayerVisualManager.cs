using System;
using UnityEngine;

public class PlayerVisualManager : MonoBehaviour
{
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

    public void UpdateVisuals()
    {
        for (int i = 0; i < Enum.GetValues(typeof(EBodyPartType)).Length; i++)
        {
            UpdatePart((EBodyPartType)i);
        }
    }

    public void UpdatePart(EBodyPartType bodyPart)
    {
        switch (bodyPart)
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
