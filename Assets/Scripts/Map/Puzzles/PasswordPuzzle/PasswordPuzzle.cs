using UnityEngine;

public class PasswordPuzzle : PuzzleLogic
{
    //* _answerDigits와 _inputDigits의 배열의 크기는 동일해야 한다.
    [SerializeField] private int[] _answerDigits;
    [SerializeField] private int[] _inputDigits;
    [SerializeField] private int _remainingChance;

    [ContextMenu("Init")]
    public override void Init()
    {
        _answerDigits = new int[4];
        _inputDigits = new int[] { 0, 0, 0, 0 };

        for (int i = 0; i < _answerDigits.Length; i++)
        {
            _answerDigits[i] = Random.Range(0, 9 + 1);
        }

        _remainingChance = 6;
    }

    [ContextMenu("CheckCorrection")]
    public override void CheckCorretion()
    {
        bool isCorrect = true;

        //* 정답 비밀번호와 입력 비밀번호가 일치 한지 확인한다.
        for (int i = 0; i < _inputDigits.Length; i++)
        {
            if (_inputDigits[i] != _answerDigits[i])
            {
                isCorrect = false;
                break;
            }
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
}
