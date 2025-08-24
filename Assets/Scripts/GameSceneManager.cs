using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }
    private Coroutine _currentCoroutine;


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

    public void LoadScene(SceneId sceneId)
    {
        if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
        StartCoroutine(LoadSceneCoroutine(sceneId));
    }

    private IEnumerator LoadSceneCoroutine(SceneId sceneId)
    {
        yield return ScreenEffectManager.Instance.FadeIn();

        SceneManager.LoadScene((int)sceneId);

        yield return ScreenEffectManager.Instance.FadeOut();
    }

    //! Test
    // private void Update() {
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         LoadScene(SceneId.StartMenu);
    //     }
    //     if (Input.GetKeyDown(KeyCode.X))
    //     {
    //         LoadScene(SceneId.ZeroFloor);
    //     }
    //     if (Input.GetKeyDown(KeyCode.C))
    //     {
    //         LoadScene(SceneId.Ending);
    //     }
    // }
}

public enum SceneId
{
    StartMenu,
    ZeroFloor,
    Ending
}