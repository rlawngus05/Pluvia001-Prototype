using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Door : InteractableObject
{
    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private CompositeCollider2D _destinationCameraConfiner;
    [SerializeField] private AudioClip _openSoundEffect;

    public override void Interact()
    {
        PlayerStateManager.Instance.AddState(PlayerState.Uncontrolable);

        SoundManager.Instance.PlaySoundEffect(_openSoundEffect);

        StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        yield return ScreenEffectManager.Instance.FadeIn(0.5f);

        PlayerController.Instance.MoveCharacter(_destinationPoint);
        CameraManager.Instance.ChangeConfiner(_destinationCameraConfiner);

        yield return new WaitForSecondsRealtime(0.5f); //! 움직이지 못하는 시간이 하드 코딩 되어 있음

        ScreenEffectManager.Instance.FadeOut(0.5f);

        PlayerStateManager.Instance.SetState(PlayerState.Idle);
    }
}
