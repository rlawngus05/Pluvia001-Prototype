using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class PuzzleLogic : MonoBehaviour, IInitializableObject
{
    protected bool isSolved;
    public bool IsSolved => isSolved;

    [SerializeField] private UnityEvent onSolvedEvents;

    private void Awake()
    {
        isSolved = false;

        // Init();
    }

    abstract public void Init();

    abstract public void CheckCorrection();

    virtual public void OnSolved()
    {
        onSolvedEvents.Invoke();

        isSolved = true;

        EtherManager.Instance.AddEtherCount();
    }
    
    public void AddOnSolvedEvent(UnityAction action){
        onSolvedEvents.AddListener(action);
    }    
}
