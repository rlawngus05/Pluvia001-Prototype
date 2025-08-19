using UnityEngine;
using System.Collections.Generic;

public class SoundEffectManager : MonoBehaviour
{
    // 미리 생성해 둘 AudioSource 개수
    [SerializeField] private int _poolSize;
    // AudioSource를 담아둘 리스트 (풀)
    private List<AudioSource> _audioSources;

    [SerializeField] private float _pitchShakeRange;
    private float _originalPitch = 1.0f;
    private float _volume = 1.0f;

    private void Awake()
    {
        _audioSources = new List<AudioSource>();
        for (int i = 0; i < _poolSize; i++)
        {
            // SoundEffectManager 게임 오브젝트에 AudioSource 컴포넌트를 필요한 만큼 추가
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            _audioSources.Add(source);
        }
    }

    /// <summary>
    /// 현재 재생 중이지 않은 AudioSource를 풀에서 찾아 반환합니다.
    /// </summary>
    private AudioSource GetAvailableAudioSource()
    {
        foreach (var source in _audioSources)
        {
            // isPlaying이 false이면 사용 가능한 것으로 간주
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // 만약 모든 AudioSource가 사용 중이라면,
        // 경고를 출력하고 null을 반환하거나, 새로 하나 더 생성할 수도 있습니다.
        Debug.LogWarning("사용 가능한 AudioSource가 없습니다. 풀 사이즈를 늘려주세요.");
        return null;
    }

    public void Play(AudioClip audioClip)
    {
        // 풀에서 비어있는 AudioSource를 가져옴
        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            // 해당 AudioSource의 pitch를 설정하고 재생
            source.pitch = _originalPitch;
            source.PlayOneShot(audioClip);
        }
    }

    public void PlayWithRandomPitch(AudioClip audioClip)
    {
        AudioSource source = GetAvailableAudioSource();
        if (source != null)
        {
            float pitchShakeValue = Random.Range(-_pitchShakeRange, _pitchShakeRange);
            source.pitch = _originalPitch + pitchShakeValue;
            source.PlayOneShot(audioClip);
        }
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