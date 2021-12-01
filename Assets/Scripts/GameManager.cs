using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    [Header("Health Parameters")]
    public GameObject healthUiPrefab;
    public GameObject[] coinPrefabs;
    public AudioClip[] coinSounds;

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
}
