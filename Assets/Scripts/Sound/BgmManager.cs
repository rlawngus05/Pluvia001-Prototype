using UnityEngine;

public class BgmManager : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip)
    {
        audioSource.Stop();

        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
