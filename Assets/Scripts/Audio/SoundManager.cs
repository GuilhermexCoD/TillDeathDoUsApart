using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static float masterVolume = 1;

    public static event EventHandler<float> onMasterVolumeChanged;

    public static void SetMasterVolume(float value)
    {
        masterVolume = value;

        onMasterVolumeChanged?.Invoke(null, masterVolume);
    }

    public static void PlaySound(AudioClip clip)
    {
        GameObject soundGo = new GameObject($"OneShot_Sound");
        AudioSource audioSource = soundGo.AddComponent<AudioSource>();
        audioSource.volume = masterVolume;

        AudioSubcriber audioSubcriber = soundGo.AddComponent<AudioSubcriber>();
        audioSubcriber.SetAudioSource(audioSource);

        audioSource.PlayOneShot(clip);
        GameObject.Destroy(soundGo, clip.length);
    }

    public static void PlaySound(AudioClip clip,bool loop)
    {
        string audioClipName = clip.name;
        GameObject soundGo = new GameObject($"{audioClipName}_Sound");
        AudioSource audioSource = soundGo.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.volume = masterVolume;
        audioSource.loop = loop;

        AudioSubcriber audioSubcriber = soundGo.AddComponent<AudioSubcriber>();
        audioSubcriber.SetAudioSource(audioSource);

        audioSource.Play();
    }
}
