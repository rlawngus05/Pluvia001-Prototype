using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLineGuiManager : MonoBehaviour
{
    private CutSceneTextLineManager _cutSceneTextLineManager;
    [SerializeField] private GameObject _gui;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private GameObject _textLineEndGuider;

    [SerializeField] private float _typeInterval;
    private bool _readyToEnd;
    private bool _isTyping;
    private Coroutine _typingCoroutine;
    private Action _finishLineObserver;

    private void Awake()
    {
        _readyToEnd = false;
        _cutSceneTextLineManager = GetComponent<CutSceneTextLineManager>();
    }

    public void Execute(CharacterLine characterLine, Action finishLineObserver)
    {
        StartCoroutine(ExectueCoroutine(characterLine, finishLineObserver));
    }

    private IEnumerator ExectueCoroutine(CharacterLine characterLine, Action finishLineObserver)
    {
        _gui.SetActive(true);
        _textLineEndGuider.SetActive(false);

        _isTyping = true;
        _typingCoroutine = _cutSceneTextLineManager.ExecuteLine(characterLine.Content, _contentText, () => { _isTyping = false; }, characterLine.ActorTypeSoundEffect);
        yield return new WaitWhile(() => _isTyping);
        _textLineEndGuider.SetActive(true);

        _finishLineObserver = finishLineObserver;
        _readyToEnd = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_readyToEnd)
            {
                _gui.SetActive(false);
                _finishLineObserver();

                _textLineEndGuider.SetActive(false);
                _readyToEnd = false;
            }

            if (_isTyping)
            {
                _cutSceneTextLineManager.SkipTyping();
            }
        }
    }
}
