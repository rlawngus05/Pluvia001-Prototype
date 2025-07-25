using System;
using UnityEngine;

public class CaesarCipherPuzzleLogic : PuzzleLogic
{
    [SerializeField] private string _answerPlainText;
    private string _answerCipherText;
    public string AnswerCipherText => _answerCipherText;
    [SerializeField] private string _hintPlainText;
    private string _hintCipherText;

    private int _maxInputTextLength;
    private int _n;
    private string _inputText;

    private Action<string> _inputTextChangeObserver;
    private Action _correctEvent;

    protected override void Awake()
    {
        base.Awake();

        _answerPlainText = _answerPlainText.ToUpper();
        _hintPlainText = _hintPlainText.ToUpper();
        _maxInputTextLength = 8;
    }

    public override void Initialize()
    {
        _answerCipherText = "";
        _hintCipherText = "";

        _n = UnityEngine.Random.Range(3, 8 + 1);

        foreach (char c in _answerPlainText)
        {
            char cipherCharacter = (char)((c - 'A' + _n) % 26 + 'A');

            _answerCipherText += cipherCharacter;
        }

        foreach (char c in _hintPlainText)
        {
            char cipherCharacter = (char)((c - 'A' + _n) % 26 + 'A');

            _hintCipherText += cipherCharacter;
        }
    }

    public override void Initiate()
    {
        ClearInputText();
    }

    public override bool CheckCorrection()
    {
        if (_inputText == _answerPlainText)
        {
            Debug.Log("기모취");
            _correctEvent();

            OnSolved();

            return true;
        }

        ClearInputText();
        return false;
    }

    public void AppendInputText(char c)
    {
        if (_inputText.Length < _maxInputTextLength)
        {
            _inputText += c;

            _inputTextChangeObserver(_inputText);
        }
    }

    public void ClearInputText()
    {
        _inputText = "";

        _inputTextChangeObserver(_inputText);
    }
    public void EraseInputText()
    {
        if (_inputText.Length > 0)
        {
            _inputText = _inputText.Substring(0, _inputText.Length - 1);

            _inputTextChangeObserver(_inputText);
        }
    }
    
    public string HintPlainText => _hintPlainText;
    public string HintCipherText => _hintCipherText;
    public void SetInputTextChangeObserver(Action<string> evt) { _inputTextChangeObserver = evt; }
    public void SetCorrectEvent(Action evt) { _correctEvent = evt; }
}
