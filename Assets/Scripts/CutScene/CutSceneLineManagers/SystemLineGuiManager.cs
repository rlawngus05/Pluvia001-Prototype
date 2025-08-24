using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class SystemLineGuiManager : MonoBehaviour
{
    [SerializeField] private GameObject _gui;
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private GameObject _textLineEndGuider;

    [SerializeField] private float _typeInterval;
    // [SerializeField] private AudioClip _typeSoundEffect;
    private bool _readyToEnd;
    private bool _isTyping;
    private Coroutine _typingCoroutine;
    private Action _finishLineObserver;
    

    private void Awake()
    {
        _readyToEnd = false;
    }

    public void Execute(SystemLine systemLine, Action finishLineObserver)
    {
        StartCoroutine(ExecuteCoroutine(systemLine, finishLineObserver));
    }

    private IEnumerator ExecuteCoroutine(SystemLine systemLine, Action finishLineObserver)
    {
        _gui.SetActive(true);
        _textLineEndGuider.SetActive(false);

        _isTyping = true;
        _typingCoroutine = StartCoroutine(TypeContent(systemLine.Content));
        yield return new WaitWhile(() => _isTyping);
        _textLineEndGuider.SetActive(true);
        _contentText.text = systemLine.Content;

        _finishLineObserver = finishLineObserver;
        _readyToEnd = true;
    }

    private IEnumerator TypeContent(string content)
    {
        _contentText.text = "";

        foreach (char c in content)
        {
            // SoundManager.Instance.PlaySoundEffectWithRandomPich(_typeSoundEffect);
            _contentText.text += c;

            yield return new WaitForSeconds(_typeInterval);
        }

        _isTyping = false;
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
                _isTyping = false;
                StopCoroutine(_typingCoroutine);
            }
        }
    }
}
