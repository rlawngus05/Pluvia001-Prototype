using UnityEngine;
using UnityEngine.UIElements;

public class PasswordPuzzleUIController : InteractableObject
{
    [SerializeField] private PasswordPuzzleUIScript passwordPuzzleUIScript;

    public override void Interact()
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
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.F)) //|| Input.GetKeyDown(KeyCode.Escape))
            {
                passwordPuzzleUIScript.Close();
            }
        }
    }
}
