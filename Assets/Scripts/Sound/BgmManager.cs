using UnityEngine;

public class BgmManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip, bool isLoop)
    {
        audioSource.loop = isLoop;

        audioSource.Stop();

        audioSource.clip = audioClip;
        audioSource.Play();
    }

    public void Stop() { audioSource.Stop(); }
    public void Pause() { audioSource.Pause(); }
}
