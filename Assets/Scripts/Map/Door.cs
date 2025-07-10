using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform connetingPoint;

    public void Interact(){
        PlayerController.Instance.MoveCharacter(connetingPoint);
    }
}
