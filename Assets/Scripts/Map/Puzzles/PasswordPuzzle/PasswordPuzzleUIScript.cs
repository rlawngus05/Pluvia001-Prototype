using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UIElements;

public class PasswordPuzzleUIScript : MonoBehaviour, IInitializableObject
{
    [SerializeField] private PasswordPuzzleLogic _puzzleLogic;
    [SerializeField] private DigitPanelStateDictWrapper _digitPanelStateDictWrapper; //* 유니티 editor 상에서 보여지는 필드
    private VisualElement _root;
    [SerializeField] private List<VisualElement> _digitPanels;
    [SerializeField] private List<VisualElement> _chanceNotifiers;
    private int _currentIndex;
    private Dictionary<DigitState, Color32> digitPanelStateDict; //* 스크립트 상에서, 사용되는 필드

    private void Awake()
    {
        _digitPanels = new List<VisualElement>();
        _chanceNotifiers = new List<VisualElement>();

        _root = GetComponent<UIDocument>().rootVisualElement;
        for (int i = 0; i < 4; i++)
        {
            VisualElement digitPanel = _root.Query<VisualElement>("DigitPanel" + i.ToString());
            _digitPanels.Add(digitPanel);
        }

        digitPanelStateDict = _digitPanelStateDictWrapper.ToDictionary();

        _puzzleLogic.SetRemainChanceOberver((int remainChance) =>
        {
            for (int i = 0; i < 6 - remainChance; i++)
            {
                if (!_chanceNotifiers[i].ClassListContains("chance-fail")) { _chanceNotifiers[i].AddToClassList("chance-fail"); }
            }

            //! 퍼즐 로직과 UI 관련 리펙토링 하기전에, 일단 작동 시키기 위한 임시 코드
            for (int i = 6 - remainChance; i < 6; i++)
            {
                if (_chanceNotifiers[i].ClassListContains("chance-fail")) { _chanceNotifiers[i].RemoveFromClassList("chance-fail"); }
            }
        });

        _puzzleLogic.SetFailObserve(() =>
        {
            _root.style.display = DisplayStyle.None;
        });
    }

    private void Update()
    {
        // digitPanelStateDict = digitPanelStateDictWrapper.ToDictionary(); //! Test : 에디터 상에서 실시간으로 색깔 조정가능하게 함.

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentIndex > 0)
            {
                ChangeSelectedDigitPanel(_currentIndex, --_currentIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentIndex < 3)
            {
                ChangeSelectedDigitPanel(_currentIndex, ++_currentIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) { _puzzleLogic.UpNumber(_currentIndex); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { _puzzleLogic.DownNumber(_currentIndex); }

        if (Input.GetKeyDown(KeyCode.Return)) { _puzzleLogic.CheckCorrection(); }
    }

    private void ChangeSelectedDigitPanel(int previousIndex, int currentIndex)
    {
        _digitPanels[previousIndex].RemoveFromClassList("digit-panel-selected");
        _digitPanels[currentIndex].AddToClassList("digit-panel-selected");
    }

    public void Init()
    {
        _root.style.display = DisplayStyle.None;
        _currentIndex = 0;
        _digitPanels[_currentIndex].AddToClassList("digit-panel-selected");

        //     //* 확인 버튼을 눌렀을 때, 정답이 맞는지 확인하는 콜백 함수 등록
        //     Button checkButton = root.Query<Button>("CheckButton");
        //     checkButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
        //     {
        //         _puzzleLogic.CheckCorrection();
        //     });

        //     //* UI창 닫기 버튼 눌렀을 때, UI 비활성화하는 콜백 함수 등록
        //     Button exitButton = root.Query<Button>("ExitButton");
        //     exitButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
        //     {
        //         root.style.display = DisplayStyle.None;
        //     });

        _chanceNotifiers.Clear();
        VisualElement chanceNotifierContainer = _root.Query<VisualElement>("ChanceNotifierContainer");
        for (int i = 0; i < 6; i++)
        {
            VisualElement chanceNotifier = chanceNotifierContainer.Query<VisualElement>(i.ToString());
            chanceNotifier.RemoveFromClassList("chance-fail");

            _chanceNotifiers.Add(chanceNotifier);
        }

        for (int i = 0; i < 4; i++)
        {
            VisualElement digitPanel = _root.Query<VisualElement>("DigitPanel" + i.ToString());

            int index = i;

            //* Logic 부분에서 입력 숫자값을 변경 했을 때, 변경된 사실을 받기 위해, Logic 객체에 Observer 함수를 추가함
            _puzzleLogic.AddInputNumberChangeObserver((int value) =>
            {
                Label number = digitPanel.Query<Label>("Number");

                number.text = value.ToString();
            });


            //* Logic 객체에서 번호판의 상태를 바꿨을 때, UI에 적용할 수 있도록 Observer 함수를 추가함
            _puzzleLogic.AddDigitStateChangeObserver((DigitState digitState) =>
            {
                Label number = digitPanel.Query<Label>("Number");

                Color32 changedColor = digitPanelStateDict[digitState];
                number.style.color = new StyleColor(changedColor);
            });
        }
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