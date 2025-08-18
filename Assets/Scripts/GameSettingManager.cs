using UnityEngine;
using UnityEngine.UI;

public class GameSettingManager : MonoBehaviour
{
    public static GameSettingManager Instance { get; private set; }
    [SerializeField] private GameObject _gui;
    [SerializeField] private Slider _bgmVolumeController;
    [SerializeField] private Slider _soundEffectVolumeController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update() {
        if (Input.anyKeyDown)
        {
            if (_gui.activeSelf)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) { return; }
                _gui.SetActive(false);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _bgmVolumeController.value = SoundManager.Instance.GetBgmVolume();
                    _soundEffectVolumeController.value = SoundManager.Instance.GetSoundEffectVolume();

                    _gui.SetActive(!_gui.activeSelf);
                }
            }
        }
    }

    public void ExitGame() { Application.Quit(); }

    public void SetBgmVolume() { SoundManager.Instance.SetBgmVolume(_bgmVolumeController.value); }

    public void SetSoundEffectVolume() { SoundManager.Instance.SetSoundEffectVolume(_soundEffectVolumeController.value); }
}