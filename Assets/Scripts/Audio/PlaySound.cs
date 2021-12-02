using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    private AudioClip clip;
    
    [SerializeField]
    private bool doNotRepeat;

    private void Awake()
    {
        Debug.Log("Awake PlaySound");
        if (clip != null)
        {
            Debug.Log("PlaySound CLIP");
            SoundManager.PlaySound(clip, !doNotRepeat);
        }
        else
        {
            Debug.LogError("NO SOUND CLIP");
        }
    }
}
