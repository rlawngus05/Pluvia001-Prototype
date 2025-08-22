using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerStateHandler(PlayerState state);

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance { get; private set; }
    private PlayerState _currentState;
    private event PlayerStateHandler _onStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

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

        _onStateChanged?.Invoke(_currentState);
    }

    public void Subscribe(PlayerStateHandler handler) { _onStateChanged += handler; }
    public void Unsubscribe(PlayerStateHandler handler) { _onStateChanged -= handler; }
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