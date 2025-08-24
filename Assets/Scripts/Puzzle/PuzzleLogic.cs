using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class PuzzleLogic : MonoBehaviour, IPuzzleObject
{
    protected bool isSolved;
    public bool IsSolved => isSolved;

    [SerializeField] private UnityEvent onSolvedEvents;

    virtual protected void Awake()
    {
        isSolved = false;
    }

    abstract public void Initialize();
    abstract public void Initiate();

    abstract public bool CheckCorrection();

    virtual public void OnSolved()
    {
        onSolvedEvents.Invoke();

        isSolved = true;
    }
    
    public void AddOnSolvedEvent(UnityAction action){
        onSolvedEvents.AddListener(action);
    }    
}

public enum PuzzleUIState
{
    Open,
    Close
}