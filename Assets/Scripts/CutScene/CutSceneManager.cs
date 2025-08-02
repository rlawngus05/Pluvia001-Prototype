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

    private CharacterLineGuiManager characterLineGuiManager;
    private SystemLineGuiManager systemLineGuiManager;

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

        characterLineGuiManager = GetComponent<CharacterLineGuiManager>();
        systemLineGuiManager = GetComponent<SystemLineGuiManager>();
    }

    public void SetScript(List<CutSceneLine> cutSceneScript) { _script = cutSceneScript.ToList(); }
    public void FinishCurrentLine()
    {
        _isLineFinised = true;
        _textBoxContainer.SetActive(false);
    }

    public void StartCutScene()
    {
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

        if (_currentLine is ActionLine actionLine) { Debug.Log("ActionLine"); }
        if (_currentLine is CharacterLine characterLine)
        {
            _textBoxContainer.SetActive(true);
            characterLineGuiManager.Execute(characterLine, FinishCurrentLine);
        }
        if (_currentLine is ChoiceLine choiceLine) { Debug.Log("ChoiceLine"); }
        if (_currentLine is SystemLine systemLine)
        {
            _textBoxContainer.SetActive(true);
            systemLineGuiManager.Execute(systemLine, FinishCurrentLine);
        }

        yield return new WaitUntil(() => _isLineFinised);

        StartCoroutine(ExecuteNextLine());
    }

    private void EndCutScene()
    {
        _script.Clear();
    }
}
