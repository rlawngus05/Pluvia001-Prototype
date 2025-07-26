using System;
using UnityEngine;

public class CaesarCipherPuzzleLogic : PuzzleLogic
{
    [SerializeField] private string _answerPlain;
    private string _answerCipher;
    public string AnswerCipher => _answerCipher;
    [SerializeField] private string _hintPlain;
    private string _hintCipher;

    private int _maxInputTextLength;
    private int _n;
    private string _inputText;

    private Action<string> _inputTextChangeObserver;
    private Action _successObserver;

    protected override void Awake()
    {
        base.Awake();

        _answerPlain = _answerPlain.ToUpper();
        _hintPlain = _hintPlain.ToUpper();
        _maxInputTextLength = 8; //! 현재 최대 입력 크기가 8로 하드코딩 됨
    }

    public override void Initialize()
    {
        _answerCipher = "";
        _hintCipher = "";

        _n = UnityEngine.Random.Range(3, 8 + 1);

        foreach (char c in _answerPlain)
        {
            char cipherCharacter = (char)((c - 'A' + _n) % 26 + 'A');

            _answerCipher += cipherCharacter;
        }

        foreach (char c in _hintPlain)
        {
            char cipherCharacter = (char)((c - 'A' + _n) % 26 + 'A');

            _hintCipher += cipherCharacter;
        }
    }

    public override void Initiate()
    {
        ClearInputText();
    }

    public override bool CheckCorrection()
    {
        if (_inputText == _answerPlain)
        {
            Debug.Log("기모취");
            _successObserver();

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
    
    public string HintPlain => _hintPlain;
    public string HintCipher => _hintCipher;
    public void SetInputTextChangeObserver(Action<string> evt) { _inputTextChangeObserver = evt; }
    public void SetSuccessObserver(Action evt) { _successObserver = evt; }
}
