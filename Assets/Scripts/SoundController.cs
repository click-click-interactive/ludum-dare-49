using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip overloadSFX;
    public AudioClip cryostasisSFX;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnOverload()
    {
        _audioSource.clip = overloadSFX;
        _audioSource.Play();
    }

    public void OnCryostasis()
    {
        _audioSource.clip = cryostasisSFX;
        _audioSource.Play();
    }
}