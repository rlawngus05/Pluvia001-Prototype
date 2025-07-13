using System;
using System.Collections.Generic;
using UnityEngine;

public class PasswordPuzzle : PuzzleLogic
{
    //* _answerDigits와 _inputDigits의 배열의 크기는 동일해야 한다.
    [SerializeField] private int[] _answerDigits;
    [SerializeField] private int[] _inputDigits;
    [SerializeField] private int _remainingChance;
    private List<Action<int>> _digitNumberChangeObervers = new List<Action<int>>(); //! 이 부분 따로 리펙토링하기
    private List<Action<DigitState>> _digitStateChangeObservers = new List<Action<DigitState>>();
    bool isFirstInit = true;
    //! Awake()함수가 PasswordPuzzleUIScript의 Awake()함수와 동시에 실행되어서, 초기화 순서 문제로 이걸 사용하는 변수임
    //! 다른 좋은 방법으로 리펙토링 할 수 있는지 확인하기

    [ContextMenu("Init")]
    public override void Init()
    {
        if (isFirstInit)
        {
            _answerDigits = new int[4];
        }
        else
        {
            foreach (Action<int> action in _digitNumberChangeObervers)
            {
                action(0);
            }
        }

        _inputDigits = new int[] { 0, 0, 0, 0 };
        for (int i = 0; i < 4; i++)
        {
            _answerDigits[i] = UnityEngine.Random.Range(0, 9 + 1);

            if (!isFirstInit)
            {
                _digitStateChangeObservers[i](DigitState.Init);
            }
        }

        _remainingChance = 6;

        isFirstInit = false;
    }

    [ContextMenu("CheckCorrection")]
    public override void CheckCorretion()
    {
        bool isCorrect = true;

        //* 정답 비밀번호와 입력 비밀번호가 일치 한지 확인한다.
        //* 정답 비밀번호와 입력 비밀번호의 차를 구하여, 각각의 번호판의 상태를 변경한다.
        for (int i = 0; i < _inputDigits.Length; i++)
        {
            if (_inputDigits[i] != _answerDigits[i])
            {
                isCorrect = false;
            }

            int difference = Math.Abs(_inputDigits[i] - _answerDigits[i]);
            DigitState digitState;

            if (difference == 0)
            {
                digitState = DigitState.Correct;
            }
            else if (difference <= 3)
            {
                digitState = DigitState.Close;
            }
            else if (difference <= 6)
            {
                digitState = DigitState.Moderate;
            }
            else
            {
                digitState = DigitState.Far;
            }

            _digitStateChangeObservers[i](digitState);
        }

        //* 정답이 맞으면, 퍼즐 완료 처리함
        //* 정답이 틀리면, 기회를 1회 줄이고, 만약 기회 모두 소진 상황시, 퍼즐 초기화
        if (isCorrect)
        {
            Debug.Log("헤으응~♥ 가버렸..!!"); //! 디버그 용도
            OnSolved();
        }
        else
        {
            if (_remainingChance == 1)
            {
                Debug.Log("ㅋ 다시 처음 부터해"); //! 디버그 용도
                Init();
            }
            else
            {
                Debug.Log("허접~ 그것 밖에 안돼? ㅋ"); //! 디버그 용도
                _remainingChance--;
            }
        }
    }

    //* 상승, 하강 버튼 눌렀을때, UI에 CallBack 메시지를 보낼 Observer를 입력 받는 메소드
    public void AddDigitNumberChangeObserver(Action<int> observerEvent)
    {
        _digitNumberChangeObervers.Add(observerEvent);
    }

    //* 상승 버튼 UI 눌렀을때, index 위치의 숫자를 1증가 시킴
    public void OnUpBottonPressed(int index)
    {
        if (_inputDigits[index] < 9)
        {
            _digitNumberChangeObervers[index](++_inputDigits[index]);
        }
    }

    //* 하강 버튼 UI 눌렀을때, index 위치의 숫자를 1감소 시킴
    public void OnDownBottonPressed(int index)
    {
        if (_inputDigits[index] > 0)
        {
            _digitNumberChangeObervers[index](--_inputDigits[index]);
        }
    }

    //* 번호판 UI의 상태를 변경함 -> 색깔 변화를 유도 함.
    public void AddDigitStateChangeObserver(Action<DigitState> observerEvent)
    {
        _digitStateChangeObservers.Add(observerEvent);
    }

}

public enum DigitState
{
    Correct,
    Close,
    Moderate,
    Far,
    Init
}
