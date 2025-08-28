using System;
using UnityEngine;

public class ActionLineFinisher : MonoBehaviour
{
    private Action _finishLineObserver;

    public void SetFinishLineObserver(Action finishLineObserver) { _finishLineObserver = finishLineObserver; }

    public void FinishLine() { _finishLineObserver(); }
}
