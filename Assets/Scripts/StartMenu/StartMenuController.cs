using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private AudioClip _gameStartSoundEffect;
    [SerializeField] private AudioClip _startMenuBgm;

    private void Start() {
        SoundManager.Instance.StopBgm();
        SoundManager.Instance.PlayBgm(_startMenuBgm);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) { return; }

            SoundManager.Instance.PlaySoundEffect(_gameStartSoundEffect);
            SoundManager.Instance.StopBgm();

            GameSceneManager.Instance.LoadScene(SceneId.ZeroFloor);
        }
    }
}
