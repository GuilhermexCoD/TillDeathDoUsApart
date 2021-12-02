using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    [Header("Health Parameters")]
    public GameObject healthUiPrefab;
    public GameObject[] coinPrefabs;
    public AudioClip[] coinSounds;

    public int totalCoin = 0;

    public event Action<int> onCoinChanged;

    private void Awake()
    {
        if (current != null)
        {
            Destroy(this.gameObject);
            return;
        }

        current = Singleton<GameManager>.Instance;
    }

    public static AudioClip GetRandomCoinAudioClip()
    {
        int totalClips = current.coinSounds.Length;

        return current.coinSounds[Random.Range(0, totalClips)];
    }

    public static GameObject GetRandomCoin()
    {
        int totalCoins = current.coinPrefabs.Length;

        return current.coinPrefabs[Random.Range(0, totalCoins)];
    }

    public static void AddCoin(int value)
    {
        current.totalCoin += value;
        current.onCoinChanged?.Invoke(current.totalCoin);
    }

    public static int GetCoin()
    {
        return current.totalCoin;
    }
}
