using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class ActionLine : CutSceneLine
{
    [SerializeField] private GameObject _timelinePrefab;
    private PlayableDirector _playableDirector;
}
