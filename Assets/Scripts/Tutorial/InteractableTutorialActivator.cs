using UnityEngine;

[RequireComponent(typeof(TutorialActivator))]
public class InteractableTutorialActivator : InteractableObject
{
    private TutorialActivator _tutorialActivator;

    protected override void Awake()
    {
        base.Awake();

        _tutorialActivator = GetComponent<TutorialActivator>();
    }

    protected override void OnInteract()
    {
        _tutorialActivator.Activate();
    }
}
