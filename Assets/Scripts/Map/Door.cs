using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private CompositeCollider2D destinationCameraConfiner;

    public void Interact()
    {
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        yield return ScreenEffectManager.Instance.FadeIn(0.5f);

        PlayerController.Instance.MoveCharacter(destinationPoint);
        CameraManager.Instance.ChangeConfiner(destinationCameraConfiner);

        yield return new WaitForSecondsRealtime(0.5f);

        ScreenEffectManager.Instance.FadeOut(0.5f);
    }
}
