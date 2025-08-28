using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TutorialActivator))]
public class DoorWithTutorialActivator : Door
{
    private TutorialActivator _tutorialActivator;

    protected override void Awake()
    {
        base.Awake();
        _tutorialActivator = GetComponent<TutorialActivator>();
    }

    protected override void OnInteract()
    {
        PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);

        StartCoroutine(TransitionRoutineWithCutSceneActivate());
    }

    private IEnumerator TransitionRoutineWithCutSceneActivate()
    {
        if (TutorialManager.Instance.HasState(_tutorialActivator.TutorialType))
        {
            yield return StartCoroutine(TransitionRoutine());
            _tutorialActivator.Activate();
        }
        else
        {
            yield return StartCoroutine(TransitionRoutine(true));
            _tutorialActivator.Activate();
        }
    }
}
