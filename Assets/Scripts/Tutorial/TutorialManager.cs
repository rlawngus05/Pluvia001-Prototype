using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<CutSceneActivator> _cutSceneActivators;
    public static TutorialManager Instance { get; private set; }
    private TutorialState _currentState;
    private event Action<TutorialState> _onStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool HasState(TutorialState state)
    {
        return (_currentState & state) == state;
    }

    private void AddState(TutorialState state)
    {
        _currentState |= state;
        OnStateChanged();
    }

    private void DeleteState(TutorialState state)
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

    public void ActivateTutorial(TutorialState type)
    {
        if (HasState(type)) { return; }

        _cutSceneActivators[(int)Math.Log((double)type, 2.0)].Activate();
        AddState(type);
    }

    public void Subscribe(Action<TutorialState> handler) { _onStateChanged += handler; }
    public void Unsubscribe(Action<TutorialState> handler) { _onStateChanged -= handler; }
}

[Flags]
public enum TutorialState
{
    Move = 1,
    Interact = 1 << 1,
    GameGoal = 1 << 2,
    Trap = 1 << 3,
    InventoryOpen = 1 << 4,
    InventoryUse = 1 << 5,
    Puzzle = 1 << 6,
    GetEther = 1 << 7
}