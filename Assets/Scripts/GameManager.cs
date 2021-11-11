using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    [Header("Health Parameters")]
    public GameObject healthUiPrefab;

    private void Awake()
    {
        if (current != null)
        {
            Destroy(this.gameObject);
            return;
        }

        current = Singleton<GameManager>.Instance;
    }
}
