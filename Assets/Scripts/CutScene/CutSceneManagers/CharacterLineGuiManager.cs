using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLineGuiManager : MonoBehaviour
{
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
    }

    public void Execute(CharacterLine characterLine, Action finishLineObserver)
    {
        StartCoroutine(ExectueCoroutine(characterLine, finishLineObserver));
    }

    private IEnumerator ExectueCoroutine(CharacterLine characterLine, Action finishLineObserver)
    {
        _gui.SetActive(true);
        _textLineEndGuider.SetActive(false);

        _portraitImage.sprite = characterLine.Portrait;
        _nameText.text = characterLine.ActorName;

        _isTyping = true;
        _typingCoroutine = StartCoroutine(TypeContent(characterLine.Content, characterLine.ActorTypeSoundEffect));
        yield return new WaitWhile(() => _isTyping);
        _textLineEndGuider.SetActive(true);
        _contentText.text = characterLine.Content;

        _finishLineObserver = finishLineObserver;
        _readyToEnd = true;
    }

    private IEnumerator TypeContent(string content, AudioClip typeSoundEffect)
    {
        _contentText.text = "";
        int cnt = 0;
        foreach (char c in content)
        {
            if (cnt++ % 2 == 0)
            {
                SoundManager.Instance.PlaySoundEffectWithRandomPich(typeSoundEffect);
            }
            _contentText.text += c;

            yield return new WaitForSeconds(_typeInterval);
        }

        _isTyping = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            if (_readyToEnd)
            {
                _gui.SetActive(false);
                _finishLineObserver();

                _textLineEndGuider.SetActive(false);
                _readyToEnd = false;

                return;
            }

            if (_isTyping)
            {
                StopCoroutine(_typingCoroutine);
                _isTyping = false;
            }
        }

    }
}
