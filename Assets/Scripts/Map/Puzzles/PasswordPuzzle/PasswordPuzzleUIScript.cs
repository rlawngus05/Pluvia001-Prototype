using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UIElements;

public class PasswordPuzzleUIScript : MonoBehaviour, IInitializableObject
{
    [SerializeField] private PasswordPuzzleLogic _puzzleLogic;
    [SerializeField] private DigitPanelStateDict digitPanelStateDictWrapper;
    private Dictionary<DigitState, Color32> digitPanelStatDict;
    
    private void Awake() {
        digitPanelStatDict = digitPanelStateDictWrapper.ToDictionary();
    }

    private void Update() {
        // digitPanelStatDict = digitPanelStateDictWrapper.ToDictionary(); //! Test : 에디터 상에서 실시간으로 색깔 조정가능하게 함.
    }

    public void Init()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.style.display = DisplayStyle.None;

        //* 확인 버튼을 눌렀을 때, 정답이 맞는지 확인하는 콜백 함수 등록
        Button checkButton = root.Query<Button>("CheckButton");
        checkButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
        {
            _puzzleLogic.CheckCorrection();
        });

        //* UI창 닫기 버튼 눌렀을 때, UI 비활성화하는 콜백 함수 등록
        Button exitButton = root.Query<Button>("ExitButton");
        exitButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
        {
            root.style.display = DisplayStyle.None;
        });

        for (int i = 0; i < 4; i++)
        {
            VisualElement digit = root.Query<VisualElement>("Digit" + i.ToString());

            Button upButton = digit.Query<Button>("UpButton");
            Button downButton = digit.Query<Button>("DownButton");

            int index = i;
            //* 숫자 상승 버튼을 눌렀을 때, Logic객체의 해당되는 부분의 숫자를 1증가 시키는 콜백 함수 등록
            upButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
            {
                _puzzleLogic.OnUpBottonPressed(index);
            });

            //* 숫자 하강 버튼을 눌렀을 때, Logic객체의 해당되는 부분의 숫자를 1감소 시키는 콜백 함수 등록
            downButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
            {
                _puzzleLogic.OnDownBottonPressed(index);
            });


            //* Logic 부분에서 입력 숫자값을 변경 했을 때, 변경된 사실을 받기 위해, Logic 객체에 Observer 함수를 추가함
            _puzzleLogic.AddDigitNumberChangeObserver((int value) =>
            {
                Label label = digit.Query<Label>("Label");

                label.text = value.ToString();
            });


            //* Logic 객체에서 번호판의 상태를 바꿨을 때, UI에 적용할 수 있도록 Observer 함수를 추가함
            _puzzleLogic.AddDigitStateChangeObserver((DigitState digitState) =>
            {
                VisualElement digitPanel = digit.Query<VisualElement>("DigitPanel");

                Color32 changedColor = digitPanelStatDict[digitState];
                digitPanel.style.backgroundColor = new StyleColor(changedColor);
            });
        }
    }
}

//* 아래 클래스들은, Dicionary의 요소를 에디터 상에서 편집할 수 있게 해주는 중간 다리 역할을 함
[Serializable]
public class DigitPanelStateItem
{
    [SerializeField] DigitState digitState;
    [SerializeField] Color32 color;

    public DigitState GetDigitState => digitState;
    public Color32 GetColor => color;

}   

[Serializable]
public class DigitPanelStateDict
{
    [SerializeField] List<DigitPanelStateItem> digitPanelStateItems;

    public Dictionary<DigitState, Color32> ToDictionary()
    {
        Dictionary<DigitState, Color32> dict = new Dictionary<DigitState, Color32>();

        foreach(DigitPanelStateItem digitPanelStateItem in digitPanelStateItems){
            dict.Add(digitPanelStateItem.GetDigitState, digitPanelStateItem.GetColor);
        }

        return dict;
    }
}