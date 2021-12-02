using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLoot : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Actor>().OnDestroyed += OnActorDestroyed;
    }

    private void OnActorDestroyed(Actor obj)
    {
        if (obj.GetComponent<HealthSystem>().GetHealthNormalized() == 0)
        {
            var prefab = GameManager.GetRandomCoin();
            Instantiate(prefab, obj.transform.position, Quaternion.identity);
        }
        this.GetComponent<Actor>().OnDestroyed -= OnActorDestroyed;
    }

}
