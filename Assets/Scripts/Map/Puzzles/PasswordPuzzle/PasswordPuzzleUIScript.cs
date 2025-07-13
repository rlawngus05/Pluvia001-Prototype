using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.UIElements;

public class PasswordPuzzleUIScript : MonoBehaviour
{
    [SerializeField] private PasswordPuzzle _puzzleLogic;

    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.style.display = DisplayStyle.None;


        //* 확인 버튼을 눌렀을 때, 정답이 맞는지 확인하는 메소드 호출
        Button checkButton = root.Query<Button>("CheckBotton");
        checkButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
        {
            _puzzleLogic.CheckCorretion();
        });

        //* UI창 종료 버튼에 이벤트 추가
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
            //* 숫자 상승 버튼을 눌렀을 때, Logic 객체의 index 자리 숫자를 1증가 시킴
            upButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
            {
                _puzzleLogic.OnUpBottonPressed(index);
            });

            //* 숫자 상승 버튼을 눌렀을 때, Logic 객체의 index 자리 숫자를 1감소 시킴
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


            //* 번호판의 상태에 따라, 번호판 뒷 배경 색깔을 바꿈
            _puzzleLogic.AddDigitStateChangeObserver((DigitState digitState) =>
            {
                VisualElement digitPanel = digit.Query<VisualElement>("DigitPanel");

                switch (digitState)
                {
                    case DigitState.Init:
                        digitPanel.style.backgroundColor = new StyleColor(new Color32(190, 190, 190, 255));
                        break;

                    case DigitState.Correct:
                        digitPanel.style.backgroundColor = new StyleColor(new Color32(80, 200, 120, 255));
                        break;

                    case DigitState.Close:
                        digitPanel.style.backgroundColor = new StyleColor(new Color32(255, 220, 80, 255));
                        break;

                    case DigitState.Moderate:
                        digitPanel.style.backgroundColor = new StyleColor(new Color32(255, 150, 60, 255));
                        break;

                    case DigitState.Far:
                        digitPanel.style.backgroundColor = new StyleColor(new Color32(230, 70, 70, 255));
                        break;

                }
            });
        }
    }
}
