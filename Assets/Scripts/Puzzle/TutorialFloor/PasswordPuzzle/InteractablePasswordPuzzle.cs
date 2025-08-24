using UnityEngine;

public class InteractablePasswordPuzzle : InteractableObject
{
    [SerializeField] private PasswordPuzzleUIScript passwordPuzzleUIScript;

    protected override void OnInteract()
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
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
            {
                passwordPuzzleUIScript.Close();
            }
        }
    }
}
