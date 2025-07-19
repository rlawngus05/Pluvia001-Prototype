using System;
using UnityEngine;

public class CaesarCipherPuzzleLogic : PuzzleLogic
{
    [SerializeField] string _plainText;
    [SerializeField] private int _maxTextLength;
    [SerializeField] int _n;
    [SerializeField] string _cipherText;
    private Action<string> _userInputTextChangeEvent;
    private Action _wrongEvent;
    private Action _correctEvent;
    private string _userInputText;

    protected override void Awake() {
        _cipherText = "";

        foreach (char c in _plainText)
        {
            char cipherCharacter = (char)((c - 'A' + _n) % 26 + 'A');

            _cipherText += cipherCharacter;
        }

        base.Awake();
    }

    [ContextMenu(nameof(Init))]
    public override void Init()
    {
        _userInputText = "";
    }

    public override bool CheckCorrection()
    {
        if (_userInputText == _cipherText)
        {
            Debug.Log("기모취");
            OnSolved();

            return true;
        }
        Debug.Log("틀림");
        return false;
    }

    public void AppendUserInputText(char c)
    {
        _userInputText += c;

        _userInputTextChangeEvent(_userInputText);
        if (_userInputText.Length == _maxTextLength)
        {
            bool isCorrect = CheckCorrection();

            if (!isCorrect)
            {
                _wrongEvent();
                Init();
            }
            else
            {
                _correctEvent();
            }
        }
    }

    public int GetMaxTextLength() { return _maxTextLength; }
    public void ClearUserInputText()
    {
        _userInputText = "";
        _wrongEvent();
    }

    public void SetUserInputTextChangeEvent(Action<string> evt)
    {
        _userInputTextChangeEvent = evt;
    }

    public void SetCorrectEvent(Action evt)
    {
        _correctEvent = evt;
    }

    public void SetWrongEvent(Action evt)
    {
        _wrongEvent = evt;
    }
}
