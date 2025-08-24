using UnityEngine;

public class BgmManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip, bool isLoop)
    {
        _audioSource.loop = isLoop;

        _audioSource.Stop();

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void SetVolume(float volume) { _audioSource.volume = volume; }
    public float GetVolume() { return _audioSource.volume; }

    public void Stop() { _audioSource.Stop(); }
    public void Pause() { _audioSource.Pause(); }
}
