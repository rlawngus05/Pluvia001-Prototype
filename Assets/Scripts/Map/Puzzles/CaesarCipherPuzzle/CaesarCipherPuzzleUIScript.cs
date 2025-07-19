using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CaesarCipherPuzzleUIScript : MonoBehaviour, IInitializableObject
{
    [SerializeField] CaesarCipherPuzzleLogic _puzzleLogic;
    private List<Label> _inputTexts;
    public int _maxTextLength;

    private void Awake()
    {
        _maxTextLength = _puzzleLogic.GetMaxTextLength(); //! 테스트 단계서에서 PlainText는 Ether로 고정됨
        _inputTexts = new List<Label>();

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.style.display = DisplayStyle.None;

        for (int i = 0; i < _maxTextLength; i++)
        {
            Label inputText = root.Query<Label>("InputText" + i.ToString());
            _inputTexts.Add(inputText);
        }

        //* UI창 닫기 버튼 눌렀을 때, UI 비활성화하는 콜백 함수 등록
        Button exitButton = root.Query<Button>("ExitButton");
        exitButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
        {
            root.style.display = DisplayStyle.None;
        });

        for (int i = 0; i < 26; i++)
        {
            char currentChar = (char)(i + 'A');
            Button alphabetButton = root.Query<Button>(currentChar.ToString());

            alphabetButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
            {
                _puzzleLogic.AppendUserInputText(currentChar);
            });
        }

        Button resetButton = root.Query<Button>("ResetButton");
        resetButton.RegisterCallback<ClickEvent>((ClickEvent evt) =>
        {
            _puzzleLogic.Init();
            Init();
        });

        _puzzleLogic.SetUserInputTextChangeEvent((string text) =>
        {
            for (int i = 0; i < text.Length; i++)
            {
                _inputTexts[i].text = text[i].ToString();
            }
        });

        _puzzleLogic.SetCorrectEvent(() =>
        {
            foreach (Label inputText in _inputTexts)
            {
                inputText.AddToClassList("correct-event");
            }
        });

        _puzzleLogic.SetWrongEvent(() =>
        {
            Init();
        });
    }

    public void Init()
    {
        foreach (Label inputText in _inputTexts)
        {
            inputText.text = "";
        }
    }
}
