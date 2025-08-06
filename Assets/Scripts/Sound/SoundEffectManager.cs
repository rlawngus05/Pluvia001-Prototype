using System;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private float _pitchShakeRange;
    private float _originalPitch;

    private void Awake()
    {
        _originalPitch = 1.0f;
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip)
    {
        _audioSource.pitch = _originalPitch;
        _audioSource.PlayOneShot(audioClip);
    }

    public void PlayWithRandomPitch(AudioClip audioClip)
    {
        float pitchShakeValue = UnityEngine.Random.Range(-_pitchShakeRange, _pitchShakeRange);

        _audioSource.pitch = _originalPitch + pitchShakeValue;
        _audioSource.PlayOneShot(audioClip);
    }
}
