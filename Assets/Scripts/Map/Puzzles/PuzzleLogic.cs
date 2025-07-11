using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

abstract public class PuzzleLogic : MonoBehaviour
{
    private bool isSolved;
    [SerializeField] private UnityEvent onSolvedEvents;

    abstract public void Init();

    virtual public void OnSolved()
    {
        onSolvedEvents.Invoke();

        EtherManager.Instance.AddEtherCount();
    }
    
    public void AddOnSolvedEvent(UnityAction action){
        onSolvedEvents.AddListener(action);
    }    
}
