using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public static CutSceneManager Instance { get; private set; }
    [SerializeReference, SubclassSelector] private List<CutSceneLine> _script;
    [SerializeReference, SubclassSelector] private CutSceneLine _currentLine;
    [SerializeField] private GameObject _textBoxContainer;

    private CharacterLineGuiManager _characterLineGuiManager;
    private SystemLineGuiManager _systemLineGuiManager;
    private ActionLineManager _actionLineManager;

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

        _characterLineGuiManager = GetComponent<CharacterLineGuiManager>();
        _systemLineGuiManager = GetComponent<SystemLineGuiManager>();
        _actionLineManager = GetComponent<ActionLineManager>();
    }

    public void SetScript(List<CutSceneLine> cutSceneScript) { _script = cutSceneScript.ToList(); }
    public void FinishCurrentLine()
    {
        _isLineFinised = true;
        _textBoxContainer.SetActive(false);
    }

    public void StartCutScene()
    {
        PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);
        _currentLineIndex = 0;

        StartCoroutine(ExecuteNextLine());
    }

    private IEnumerator ExecuteNextLine()
    {
        if (_currentLineIndex >= _script.Count)
        {
            EndCutScene();
            yield break;
        }

        _isLineFinised = false;

        _currentLine = _script[_currentLineIndex++];

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
        PlayerStateManager.Instance.SetState(PlayerState.Idle);

        _script.Clear();
    }
}
