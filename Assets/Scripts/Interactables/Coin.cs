using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Interactable
{
    public override IInteractable PickUp(object instigator)
    {
        var clip = GameManager.GetRandomCoinAudioClip();
        SoundManager.PlaySound(clip);
        return base.PickUp(instigator);
    }
}
