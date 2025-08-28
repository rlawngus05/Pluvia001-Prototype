using UnityEngine;

public class TutorialActivator : MonoBehaviour
{
    [SerializeField] private TutorialState _tutorialType;
    public TutorialState TutorialType => _tutorialType;

    public void Activate()
    {
        TutorialManager.Instance.ActivateTutorial(_tutorialType);
    }
}
