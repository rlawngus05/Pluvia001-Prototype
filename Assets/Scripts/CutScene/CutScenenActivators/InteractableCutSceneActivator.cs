using UnityEngine;

[RequireComponent(typeof(CutSceneActivator))]
public class InteractableCutSceneActivator : InteractableObject
{
    private CutSceneActivator _cutSceneActivator;

    protected override void Awake()
    {
        base.Awake();
        _cutSceneActivator = GetComponent<CutSceneActivator>();
    }

    protected override void OnInteract()
    {
        _cutSceneActivator.Activate();
    }
}
