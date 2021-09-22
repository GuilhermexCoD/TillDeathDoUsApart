using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static float masterVolume = 1;

    public static void SetMasterVolume(float value)
    {
        masterVolume = value;
    }

    public static void PlaySound(AudioClip clip)
    {
        //string audioClipName = clip.name;
        GameObject soundGo = new GameObject($"OneShot_Sound");
        AudioSource audioSource = soundGo.AddComponent<AudioSource>();
        audioSource.volume = masterVolume;

        audioSource.PlayOneShot(clip);
    }
}
