using UnityEngine;
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour
{
    // 미리 생성해 둘 AudioSource 개수
    [SerializeField] private int _initialPoolSize;
    // AudioSource를 담아둘 리스트 (풀)
    private List<AudioSource> _audioSources;

    [SerializeField] private float _pitchShakeRange;
    private float _originalPitch;
    private float _volume;

    private void Awake()
    {
        _audioSources = new List<AudioSource>();
        _originalPitch = 1.0f;
        _volume = 1.0f;

        for (int i = 0; i < _initialPoolSize; i++)
        {
            // SoundEffectManager 게임 오브젝트에 AudioSource 컴포넌트를 필요한 만큼 추가
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            _audioSources.Add(source);
        }
    }

    /// <summary>
    /// 현재 재생 중이지 않은 AudioSource를 풀에서 찾아 반환합니다.
    /// 만약 모두 사용 중이면 새로 생성하여 풀에 추가하고 반환합니다.
    /// </summary>
    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in _audioSources)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // --- 여기부터가 추가된 부분 ---
        // 사용 가능한 소스가 없다면 새로 하나 생성
        Debug.Log("풀이 부족하여 새로운 AudioSource를 동적으로 추가합니다.");
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.playOnAwake = false;
        newSource.volume = _volume; // 현재 볼륨 설정 적용
        _audioSources.Add(newSource); // 생성된 소스를 풀에 추가
        
        return newSource; // 새로 생성된 소스를 즉시 반환
    }

    public void Play(AudioClip audioClip)
    {
        if (audioClip == null) { return; }
        // 풀에서 비어있는 AudioSource를 가져옴
        AudioSource source = GetAvailableAudioSource();
        // 해당 AudioSource의 pitch를 설정하고 재생
        source.pitch = _originalPitch;
        source.PlayOneShot(audioClip);
    }

    public void PlayWithRandomPitch(AudioClip audioClip)
    {
        if (audioClip == null) { return; }

        AudioSource source = GetAvailableAudioSource();
        float pitchShakeValue = Random.Range(-_pitchShakeRange, _pitchShakeRange);
        source.pitch = _originalPitch + pitchShakeValue;
        source.PlayOneShot(audioClip);
    }

    // 볼륨 설정 시, 풀에 있는 모든 AudioSource의 볼륨을 조절해야 합니다.
    public void SetVolume(float volume)
    {
        _volume = volume;
        foreach (var source in _audioSources)
        {
            source.volume = _volume;
        }
    }

    public float GetVolume()
    {
        return _volume;
    }
}