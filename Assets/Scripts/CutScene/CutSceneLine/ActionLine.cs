using System;
using UnityEngine;

[Serializable]
public class ActionLine : CutSceneLine
{
    [SerializeField] private bool _isSetter;
    public bool IsSetter => _isSetter;

    [SerializeField] private string _playableDirectorId;
    public string PlayableDirectorId => _playableDirectorId;
}
