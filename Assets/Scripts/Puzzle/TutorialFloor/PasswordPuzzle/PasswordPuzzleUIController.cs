using UnityEngine;

public class PasswordPuzzleUIController : InteractableObject
{
    [SerializeField] private PasswordPuzzleUIScript passwordPuzzleUIScript;

    public override void Interact()
    {
        base.Interact();
        
        if (passwordPuzzleUIScript.GetState() == PuzzleUIState.Close)
        {
            passwordPuzzleUIScript.Open();
        }
    }

    private void Update()
    {
        if (passwordPuzzleUIScript.GetState() == PuzzleUIState.Open)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
            {
                passwordPuzzleUIScript.Close();
            }
        }
    }
}
