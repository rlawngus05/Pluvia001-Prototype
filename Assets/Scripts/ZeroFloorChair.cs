using System.Collections.Generic;
using UnityEngine;

public class ZeroFloorChair : InteractableObject
{
    [SerializeField] private List<CutSceneActivator> _cutSceneActivators;
    private int _maxCutSceneAcrivatorsCount;
    private int _interactCount;

    protected override void Awake()
    {
        base.Awake();

        _maxCutSceneAcrivatorsCount = _cutSceneActivators.Count;
        _interactCount = 1;
    }

    protected override void OnInteract()
    {
        if (_interactCount == _maxCutSceneAcrivatorsCount) { base.UnsetInteractable(); }
        _cutSceneActivators[_interactCount++ - 1].Activate();
    }
}
