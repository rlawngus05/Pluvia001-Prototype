using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CaesarCipherPuzzleUIScript : MonoBehaviour, IPuzzleObject
{
    [SerializeField] CaesarCipherPuzzleLogic _puzzleLogic;
    [SerializeField] UIDocument _uiDocument;
    [SerializeField] TextMeshProUGUI _answerCipherText;

    private VisualElement _root;
    private Label _hintPlainText;
    private Label _hintCipherText;
    private Label _inputText;

    private PuzzleUIState _currentState;
    public PuzzleUIState GetState() { return _currentState; }

    private void Awake()
    {
        _root = _uiDocument.rootVisualElement;

        _hintPlainText = _root.Query<Label>("HintPlainText");
        _hintCipherText = _root.Query<Label>("HintCipherText");
        _inputText = _root.Query<Label>("InputText");
    }

    public void Initialize()
    {
        _hintPlainText.text = _puzzleLogic.HintPlain;
        _hintCipherText.text = _puzzleLogic.HintCipher;
        _answerCipherText.text = _puzzleLogic.AnswerCipher;

        _puzzleLogic.SetInputTextChangeObserver((string value) =>
        {
            _inputText.text = value;
        });

        _puzzleLogic.SetSuccessObserver(() =>
        {
            Debug.Log("UI에서도 반응함..♥");
        });
    }

    public void Initiate()
    {
        _root.style.display = DisplayStyle.None;
        _currentState = PuzzleUIState.Close;
    }

    private void Update()
    {
        if (_currentState == PuzzleUIState.Open)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                _puzzleLogic.EraseInputText();
            }

            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    if (keyCode >= KeyCode.A && keyCode <= KeyCode.Z)
                    {
                        char inputCharacter = (char)('A' + keyCode - KeyCode.A);
                        _puzzleLogic.AppendInputText(inputCharacter);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                _puzzleLogic.CheckCorrection();
            }
        }
    }
    
    public void Open()
    {
        StartCoroutine(OpenCoroutine());
    }
    //* UI 열려고 F키 눌렀을 때, 이 인식이 문자 입력으로 들어가는것을 막기 위한 보조 함수
    private IEnumerator OpenCoroutine()
    {
        yield return null;
        _root.style.display = DisplayStyle.Flex;

        PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);
        _currentState = PuzzleUIState.Open;
    }

    public void Close()
    {
        _root.style.display = DisplayStyle.None;

        PlayerStateManager.Instance.SetState(PlayerState.Idle);
        _currentState = PuzzleUIState.Close;
    }
}
