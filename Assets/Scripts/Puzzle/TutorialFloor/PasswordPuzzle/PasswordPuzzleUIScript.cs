using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPuzzleUIScript : MonoBehaviour, IPuzzleObject
{
    [SerializeField] private PasswordPuzzleLogic _puzzleLogic;
    [SerializeField] private GameObject _gui;
    private Vector2 originalPos;
    RectTransform _rectTransform;
    
    [SerializeField] private Image _panel;
    [SerializeField] private List<Sprite> _panelSprites;
    [SerializeField] private List<GameObject> _slotScrolls;
    [SerializeField] private float _slotHeight;
    [SerializeField] private List<Sprite> _digitSprites;
    [SerializeField] private List<Image> _lights;
    [SerializeField] private List<Sprite> _lightSprites;

    [SerializeField] private float _slotScrollTime;
    private bool _isScrolling;
    
    [SerializeField] private float _shakePower;
    [SerializeField] private float _shakeTime;
    private Coroutine _currentShakeEffectCoroutine; 

    private int _currentIndex;
    

    private PuzzleUIState _currentState;
    public PuzzleUIState GetState() { return _currentState; }
    public void SetState(PuzzleUIState state)
    {
        StartCoroutine(SetStateCoroutine(state));
    }
    private IEnumerator SetStateCoroutine(PuzzleUIState state)
    {
        yield return null;
        _currentState = state;
    }

    [SerializeField] private DigitPanelStateDictWrapper _digitPanelStateDictWrapper; //* 유니티 editor 상에서 보여지는 필드
    private Dictionary<DigitState, Color32> digitPanelStateDict; //* 스크립트 상에서, 사용되는 필드

    private void Awake()
    {
        digitPanelStateDict = _digitPanelStateDictWrapper.ToDictionary();
        _rectTransform = _gui.GetComponent<RectTransform>();

        originalPos = _rectTransform.position;
        _isScrolling = false;
    }

    public void Initialize()
    {
        _currentState = PuzzleUIState.Close;

        for (int i = 0; i < 4; i++)
        {
            int index = i;

            //* Logic 부분에서 입력 숫자값을 변경 했을 때, 변경된 사실을 받기 위해, Logic 객체에 Observer 함수를 추가함
            _puzzleLogic.AddInputNumberChangeObserver((int prev, int cur) =>
            {
                StartCoroutine(SlotNumberScrollingAnimation(prev, cur, index));
            });

            //* Logic 객체에서 번호판의 상태를 바꿨을 때, UI에 적용할 수 있도록 Observer 함수를 추가함
            _puzzleLogic.AddDigitStateChangeObserver((DigitState digitState) =>
            {
                Color32 changedColor = digitPanelStateDict[digitState];

                GameObject slotScroll = _slotScrolls[index];
                for (int i = 0; i < slotScroll.transform.childCount; i++)
                {
                    Image numberImage = slotScroll.transform.GetChild(i).GetChild(0).GetComponent<Image>();
                    numberImage.color = changedColor;
                }
            });
        }

        _puzzleLogic.SetRemainChanceOberver((int remainChance) =>
        {
            for (int i = 0; i < 6 - remainChance; i++)
            {
                _lights[i].sprite = _lightSprites[1];
            }

            for (int i = 6 - remainChance; i < 6; i++)
            {
                _lights[i].sprite = _lightSprites[0];
            }
        });

        _puzzleLogic.SetWrongObserver(() =>
        {
            if (_currentShakeEffectCoroutine != null)
            {
                _rectTransform.position = originalPos;
                StopCoroutine(_currentShakeEffectCoroutine);
            }
            _currentShakeEffectCoroutine = StartCoroutine(ShakeEffect());
        });

        _puzzleLogic.SetFailObserver(() =>
        {
            Close();
            Initiate();
        });

        _puzzleLogic.AddOnSolvedEvent(() =>
        {
            Debug.Log("성공 효과 실행");
            Close();
        });
    }

    public void Initiate()
    {
        _currentIndex = 0;
        ChangeSelectedDigitPanel(_currentIndex);

        foreach (Image light in _lights)
        {
            light.sprite = _lightSprites[0];
        }
    }

    private void Update()
    {
        // digitPanelStateDict = digitPanelStateDictWrapper.ToDictionary(); //! Test : 에디터 상에서 실시간으로 색깔 조정가능하게 함.

        if (_currentState == PuzzleUIState.Open && !_puzzleLogic.IsSolved)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_currentIndex > 0)
                {
                    ChangeSelectedDigitPanel(--_currentIndex);
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_currentIndex < 3)
                {
                    ChangeSelectedDigitPanel(++_currentIndex);
                }
            }

            if (!_isScrolling)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow)) { _puzzleLogic.UpNumber(_currentIndex); }
                if (Input.GetKeyDown(KeyCode.DownArrow)) { _puzzleLogic.DownNumber(_currentIndex); }
            }

            if (Input.GetKeyDown(KeyCode.Return)) { _puzzleLogic.CheckCorrection(); }
        }
    }
    
    private IEnumerator SlotNumberScrollingAnimation(int prev, int cur, int index)
    {
        _isScrolling = true;

        RectTransform rectTransform = _slotScrolls[index].GetComponent<RectTransform>();

        float from = -(-_slotHeight * prev);
        float to = -(-_slotHeight * cur);

        float elapsed = .0f;

        while (elapsed <= _slotScrollTime)
        {
            elapsed += Time.deltaTime;

            float curOffsetMax = Mathf.Lerp(from, to, EasingFunctions.EaseOutQuint(elapsed / _slotScrollTime));

            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, curOffsetMax);

            yield return null;
        }

        _isScrolling = false;
    }

    private void ChangeSelectedDigitPanel(int currentIndex)
    {
        _panel.sprite = _panelSprites[currentIndex];
    }
    private IEnumerator ShakeEffect()
    {
        originalPos = _rectTransform.position;

        float elapsed = .0f;

        while (elapsed <= _shakeTime)
        {
            elapsed += Time.deltaTime;

            Vector2 shakingPos = new Vector2(UnityEngine.Random.Range(.0f, 1.0f), UnityEngine.Random.Range(.0f, 1.0f)) * _shakePower;

            _rectTransform.position = originalPos + shakingPos;

            yield return null;
        }

        _rectTransform.position = originalPos;
    }

    public void Open()
    {
        _gui.SetActive(true);

        PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);
        SetState(PuzzleUIState.Open);
    }

    public void Close()
    {
        _gui.SetActive(false);

        PlayerStateManager.Instance.SetState(PlayerState.Idle);
        SetState(PuzzleUIState.Close);
    }
}

//* 아래 클래스들은, Dicionary의 요소를 에디터 상에서 편집할 수 있게 해주는 중간 다리 역할을 함
[Serializable]
public class DigitPanelStateItem
{
    [SerializeField] private DigitState digitState;
    [SerializeField] private Color32 color;

    public DigitState GetDigitState => digitState;
    public Color32 GetColor => color;

}   

[Serializable]
public class DigitPanelStateDictWrapper
{
    [SerializeField] private List<DigitPanelStateItem> digitPanelStateItems;

    public Dictionary<DigitState, Color32> ToDictionary()
    {
        Dictionary<DigitState, Color32> dict = new Dictionary<DigitState, Color32>();

        foreach(DigitPanelStateItem digitPanelStateItem in digitPanelStateItems){
            dict.Add(digitPanelStateItem.GetDigitState, digitPanelStateItem.GetColor);
        }

        return dict;
    }
}