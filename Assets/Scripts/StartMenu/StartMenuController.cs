using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private AudioClip _gameStartSoundEffect;
    [SerializeField] private AudioClip _startMenuBgm;
    private bool _hasStarted;

    private void Start()
    {
        _hasStarted = false;
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayBgm(_startMenuBgm);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (_hasStarted) { return; }
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Escape)) { return; }

            SoundManager.Instance.PlaySoundEffect(_gameStartSoundEffect);
            SoundManager.Instance.StopBgm();

            _hasStarted = true;
            GameSceneManager.Instance.LoadScene(SceneId.ZeroFloor);
        }
    }
}
