using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ActionLine : CutSceneLine
{
    [SerializeField] private string _playableDirectorId;
    public string PlayableDirectorId => _playableDirectorId;
}
