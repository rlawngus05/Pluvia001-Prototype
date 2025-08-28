using UnityEngine;

public class ToEasterEggController : MonoBehaviour
{
    [SerializeField] private InteractableObject _door;
    [SerializeField] private InteractableObject _fuck;
    [SerializeField] private InteractableObject _cutSceneActivator;

    private void Start()
    {
        _cutSceneActivator.UnsetInteractable();
    }

    public void LockDoor()
    {
        _door.UnsetInteractable();
        _fuck.UnsetInteractable();
        _cutSceneActivator.SetInteractable();
    }
}
