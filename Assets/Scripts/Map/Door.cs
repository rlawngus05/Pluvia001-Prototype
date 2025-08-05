using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : InteractableObject
{
    [SerializeField] private Transform destinationPoint;
    [SerializeField] private CompositeCollider2D destinationCameraConfiner;

    public override void Interact()
    {
        PlayerController.Instance.SetState(PlayerState.MoveArea);
        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        yield return ScreenEffectManager.Instance.FadeIn(0.5f);

        PlayerController.Instance.MoveCharacter(destinationPoint);
        CameraManager.Instance.ChangeConfiner(destinationCameraConfiner);

        yield return new WaitForSecondsRealtime(0.5f); //! 움직이지 못하는 시간이 하드 코딩 되어 있음

        ScreenEffectManager.Instance.FadeOut(0.5f);

        PlayerController.Instance.SetState(PlayerState.Idle);
    }
}
