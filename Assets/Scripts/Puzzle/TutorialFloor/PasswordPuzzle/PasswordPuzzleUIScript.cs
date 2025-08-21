using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPuzzleUIScript : MonoBehaviour, IPuzzleObject
{
    [SerializeField] private GameObject _gui;
    [SerializeField] private PasswordPuzzleLogic _puzzleLogic;
    private PasswordPuzzleEffects _puzzleEffects;
    
    [SerializeField] private Image _panel;
    [SerializeField] private List<Sprite> _panelSprites;
    [SerializeField] private List<Image> _lights;
    [SerializeField] private List<Sprite> _lightSprites;

    [SerializeField] private InteractableEther _interactableEther;

    private bool _isScrolling;
    private int _currentIndex;

    private PuzzleUIState _currentState;
    public PuzzleUIState GetState() { return _currentState; }
    public void SetState(PuzzleUIState state) { StartCoroutine(SetStateCoroutine(state)); }
    private IEnumerator SetStateCoroutine(PuzzleUIState state)
    {
        yield return null;
        _currentState = state;
    }

    [SerializeField] private DigitPanelStateDictWrapper _digitPanelStateDictWrapper; //* 유니티 editor 상에서 보여지는 필드
    private Dictionary<DigitState, Color32> digitPanelStateDict; //* 스크립트 상에서, 사용되는 필드

    private void Awake()
    {
        _interactableEther.UnsetInteractable();

        digitPanelStateDict = _digitPanelStateDictWrapper.ToDictionary();
        _puzzleEffects = GetComponent<PasswordPuzzleEffects>();

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
                StartCoroutine(SlotNumberScrolling(prev, cur, index));
            });

            //* Logic 객체에서 번호판의 상태를 바꿨을 때, UI에 적용할 수 있도록 Observer 함수를 추가함
            _puzzleLogic.AddDigitStateChangeObserver((DigitState digitState) =>
            {
                Color32 changedColor = digitPanelStateDict[digitState];

                _puzzleEffects.SetScrollColor(index, changedColor);
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
            _puzzleEffects.ShakeEffect();
        });

        _puzzleLogic.SetFailObserver(() =>
        {
            ExecuteFailEvent();
        });

        _puzzleLogic.AddOnSolvedEvent(() =>
        {
            StartCoroutine(ExecuteSolvedEvent());
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

    private IEnumerator SlotNumberScrolling(int prev, int cur, int index)
    {
        _isScrolling = true;

        yield return _puzzleEffects.SlotNumberScrollingAnimation(prev, cur, index);

        _isScrolling = false;
    }

    private void ChangeSelectedDigitPanel(int currentIndex)
    {
        _panel.sprite = _panelSprites[currentIndex];
    }
    
    private IEnumerator ExecuteSolvedEvent()
    {
        _panel.sprite = _panelSprites[4];
        SetState(PuzzleUIState.Close);

        _interactableEther.StopFloating();
        yield return _puzzleEffects.SpinMagicCircle();
        Close();
        yield return _puzzleEffects.ElevateEtherContainer();
        yield return _puzzleEffects.ShowEther();
        _interactableEther.SetInteractable();
        _interactableEther.ExecuteFloating();
    }

    private void ExecuteFailEvent()
    {
        Close();

        _puzzleEffects.PlayFailSequence();

        Initiate();
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