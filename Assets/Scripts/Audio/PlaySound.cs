using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;

    private void Awake()
    {
        SoundManager.PlaySound(clip, true);
    }
}
