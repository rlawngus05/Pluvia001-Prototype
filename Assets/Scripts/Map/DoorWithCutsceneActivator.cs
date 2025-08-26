using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CutSceneActivator))]
public class DoorWithCutsceneActivator : Door
{
    private CutSceneActivator _cutSceneActivator;

    protected override void Awake()
    {
        base.Awake();
        _cutSceneActivator = GetComponent<CutSceneActivator>();
    }

    protected override void OnInteract()
    {
        PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);

        StartCoroutine(TransitionRoutineWithCutSceneActivate());
    }

    private IEnumerator TransitionRoutineWithCutSceneActivate()
    {
        if (_cutSceneActivator.HasActivated || _cutSceneActivator.IsRepeatable)
        {
            yield return StartCoroutine(TransitionRoutine());
            _cutSceneActivator.Activate();
        }
        else
        {
            yield return StartCoroutine(TransitionRoutine(true));
            _cutSceneActivator.Activate();
        }
    }
}
