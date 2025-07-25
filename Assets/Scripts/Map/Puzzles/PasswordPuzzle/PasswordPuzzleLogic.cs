using System;
using System.Collections.Generic;
using UnityEngine;

public class PasswordPuzzleLogic : PuzzleLogic
{
    //* _answerDigits와 _inputDigits의 배열의 크기는 동일해야 한다.
    [SerializeField] private int[] _answerDigits;
    [SerializeField] private int[] _inputDigits;
    [SerializeField] private int _remainChance;

    private List<Action<int>> _digitNumberChangeObervers;
    private List<Action<DigitState>> _digitStateChangeObservers;
    private Action<int> _remainChanceObserver;
    private Action _failObserver;

    protected override void Awake()
    {
        base.Awake();

        _answerDigits = new int[4];
        _inputDigits = new int[4];

        _digitNumberChangeObervers = new List<Action<int>>();
        _digitStateChangeObservers = new List<Action<DigitState>>();
    }

    public override void Initialize()
    {
    }

    public override void Initiate()
    {
        for (int i = 0; i < _inputDigits.Length; i++)
        {
            _inputDigits[i] = 0;

            _digitNumberChangeObervers[i](_inputDigits[i]);
        }

        for (int i = 0; i < _answerDigits.Length; i++)
        {
            _answerDigits[i] = UnityEngine.Random.Range(0, 9 + 1);

            _digitStateChangeObservers[i](DigitState.Init);
        }

        _remainChance = 6;
    }

    [ContextMenu("CheckCorrection")]
    public override bool CheckCorrection()
    {
        bool isCorrect = true;

        for (int i = 0; i < _inputDigits.Length; i++)
        {
            //* 정답 비밀번호와 입력 비밀번호가 일치한지 확인한다.
            if (_inputDigits[i] != _answerDigits[i])
            {
                isCorrect = false;
            }

            int difference = Math.Abs(_inputDigits[i] - _answerDigits[i]);
            DigitState digitState;

            //* 정답 비밀번호와 입력 비밀번호의 차를 구하여, 각각의 번호판의 상태를 변경한다.
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
            Debug.Log("헤으응~♥ 가버렸..!!"); //! Test
            OnSolved();
        }
        else
        {
            _remainChanceObserver(--_remainChance);
            if (_remainChance == 0)
            {
                Debug.Log("ㅋ 다시 처음 부터해"); //! Test
                _failObserver();
                Initiate();
            }
            else
            {
                Debug.Log("허접~ 그것 밖에 안돼? ㅋ"); //! Test
            }
        }

        return isCorrect;
    }

    public void UpNumber(int index)
    {
        if (_inputDigits[index] < 9) { _digitNumberChangeObervers[index](++_inputDigits[index]); }
    }

    public void DownNumber(int index)
    {
        if (_inputDigits[index] > 0) { _digitNumberChangeObervers[index](--_inputDigits[index]); }
    }

    public void AddInputNumberChangeObserver(Action<int> observerEvent) { _digitNumberChangeObervers.Add(observerEvent); }
    public void AddDigitStateChangeObserver(Action<DigitState> observerEvent) { _digitStateChangeObservers.Add(observerEvent); }
    public void SetRemainChanceOberver(Action<int> oberverEvent) { _remainChanceObserver = oberverEvent; }
    public void SetFailObserver(Action observerEvent) { _failObserver = observerEvent; }
}

//* 번호판의 상태를 다루는 열거형
public enum DigitState
{
    Correct,
    Close,
    Moderate,
    Far,
    Init
}
