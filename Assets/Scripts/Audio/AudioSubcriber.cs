using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSubcriber : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        SoundManager.onMasterVolumeChanged += OnVolumeMasterChanged;
    }

    private void OnVolumeMasterChanged(object sender, float value)
    {
        audioSource.volume = value;
    }

    public void SetAudioSource(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    private void OnDestroy()
    {
        SoundManager.onMasterVolumeChanged -= OnVolumeMasterChanged;
    }
}
