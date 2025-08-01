using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private BgmManager _bgmManager;
    [SerializeField] private SoundEffectManager _soundEffectManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBgm(AudioClip audioClip) { _bgmManager.Play(audioClip); }
    public void PlaySoundEffect(AudioClip audioClip) { _soundEffectManager.Play(audioClip); }

    //! Test
    [SerializeField] private AudioClip _testBgm;
    [SerializeField] private AudioClip _testSoundEffect;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayBgm(_testBgm);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlaySoundEffect(_testSoundEffect);
        }
    }
}
