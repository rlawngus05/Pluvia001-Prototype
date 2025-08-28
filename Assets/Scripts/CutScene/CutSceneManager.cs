using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance { get; private set; }

    private Queue<CutSceneScript> _scriptQueue;
    [SerializeReference, SubclassSelector] private List<CutSceneLine> _currentScript;
    [SerializeReference, SubclassSelector] private CutSceneLine _currentLine;
    [SerializeField] private GameObject _textBoxContainer;

    private CharacterLineGuiManager _characterLineGuiManager;
    private SystemLineGuiManager _systemLineGuiManager;
    private ActionLineManager _actionLineManager;

    private bool _isCutscenePlaying;
    private bool _isLineFinised;
    private int _currentLineIndex;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _isCutscenePlaying = false;
        _scriptQueue = new Queue<CutSceneScript>();
        _characterLineGuiManager = GetComponent<CharacterLineGuiManager>();
        _systemLineGuiManager = GetComponent<SystemLineGuiManager>();
        _actionLineManager = GetComponent<ActionLineManager>();
    }

    public void SetCurrentScenePlayableDirectors(List<PlayableDirector> playableDirectors) { _actionLineManager.CurrentScenePlayableDirectors = playableDirectors; }

    public void FinishCurrentLine()
    {
        _isLineFinised = true;
        _textBoxContainer.SetActive(false);
    }

    public void EnqueueScript(CutSceneScript script)
    {
        if (_isCutscenePlaying)
        {
            _scriptQueue.Enqueue(script);
            return;
        }

        PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);
        _isCutscenePlaying = true;
        StartCutScene(script);
    }

    private void StartCutScene(CutSceneScript script)
    {
        _currentScript = script.Lines;
        _currentLineIndex = 0;
        StartCoroutine(ExecuteNextLine());
    }

    private IEnumerator ExecuteNextLine()
    {
        if (_currentLineIndex >= _currentScript.Count)
        {
            EndCutScene();
            yield break;
        }
        _isLineFinised = false;

        _currentLine = _currentScript[_currentLineIndex++];

        if (_currentLine is ActionLine actionLine)
        {
            _actionLineManager.Execute(actionLine, FinishCurrentLine);
        }
        if (_currentLine is CharacterLine characterLine)
        {
            _textBoxContainer.SetActive(true);
            _characterLineGuiManager.Execute(characterLine, FinishCurrentLine);
        }
        if (_currentLine is ChoiceLine choiceLine) { Debug.Log("ChoiceLine"); }
        if (_currentLine is SystemLine systemLine)
        {
            _textBoxContainer.SetActive(true);
            _systemLineGuiManager.Execute(systemLine, FinishCurrentLine);
        }

        yield return new WaitUntil(() => _isLineFinised);

        StartCoroutine(ExecuteNextLine());
    }

    private void EndCutScene()
    {
        if (_scriptQueue.Count() != 0)
        {
            StartCutScene(_scriptQueue.Dequeue());
            return;
        }

        _isCutscenePlaying = false;
        PlayerStateManager.Instance.SetState(PlayerState.Idle);
    }
}
