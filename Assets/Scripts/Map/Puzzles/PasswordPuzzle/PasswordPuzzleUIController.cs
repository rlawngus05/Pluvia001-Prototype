using UnityEngine;
using UnityEngine.UIElements;

public class PasswordPuzzleUIController : MonoBehaviour, IInteractable
{
    [SerializeField] private PasswordPuzzleUIScript passwordPuzzleUIScript;

    public void Interact()
    {
        if (passwordPuzzleUIScript.GetState() == PuzzleUIState.Close)
        {
            passwordPuzzleUIScript.Open();
        }
    }

    private void Update()
    {
        if (passwordPuzzleUIScript.GetState() == PuzzleUIState.Open)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.Escape))
            {
                passwordPuzzleUIScript.Close();
            }
        }
    }
}
