using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }
    private PlayerState _currentState;
    private List<Action<PlayerState>> _onStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _onStateChanged = new List<Action<PlayerState>>();
        Instance = this;
    }

    public PlayerState GetState()
    {
        return _currentState;
    }

    public bool HasState(PlayerState state)
    {
        return (_currentState & state) == state;
    }

    public void SetState(PlayerState state)
    {
        _currentState = state;
        OnStateChanged();
    }

    public void AddState(PlayerState state)
    {
        _currentState |= state;
        OnStateChanged();
    }

    public void DeleteState(PlayerState state)
    {
        _currentState &= ~state;
        OnStateChanged();
    }

    private void OnStateChanged()
    {
        StartCoroutine(OnStateChangedCoroutine());
    }

    private IEnumerator OnStateChangedCoroutine()
    {
        yield return null;

        foreach (Action<PlayerState> evt in _onStateChanged)
        {
            evt.Invoke(_currentState);
        }
    }

    public void Subscribe(Action<PlayerState> evt)
    {
        _onStateChanged.Add(evt);
    }
    
    // TODO : event 키워드 이용해서 UnSubscribe() 함수 구현하기
}

[Flags]
public enum PlayerState
{
    Idle = 0, //* Idle은 AddState()와 DeleteState()에서 쓰는 것은 의미가 없음
    Unhandlable = 1 << 1,
    Unmovable = 1 << 2,
    Uninteractable = 1 << 3,
    Uncontrolable = Unhandlable | Unmovable | Uninteractable
}