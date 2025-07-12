using UnityEngine;

public class PasswordPuzzleInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject GUI;

    public void Interact()
    {
        if (GUI.activeSelf)
        {
            GUI.SetActive(false);
        }
        else
        {
            GUI.SetActive(true);
        }
    }
}
