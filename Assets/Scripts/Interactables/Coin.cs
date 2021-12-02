using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Interactable
{
    private void Awake()
    {
        Level.current.onClear += OnLevelClear;
    }

    private void OnLevelClear()
    {
        Destroy(this.gameObject);
    }

    public override IInteractable PickUp(object instigator)
    {
        var clip = GameManager.GetRandomCoinAudioClip();
        GameManager.AddCoin(GetData<ItemData>().value);
        SoundManager.PlaySound(clip);
        return base.PickUp(instigator);
    }

    private void OnDisable()
    {
        Level.current.onClear -= OnLevelClear;
    }

    private void OnDestroy()
    {
        if (Level.current != null)
        {
            Level.current.onClear -= OnLevelClear;
        }
    }
}
