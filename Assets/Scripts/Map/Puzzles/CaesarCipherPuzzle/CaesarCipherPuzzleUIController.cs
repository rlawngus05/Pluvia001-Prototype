using UnityEngine;
using UnityEngine.UIElements;

public class CaesarCipherPuzzleUIController : MonoBehaviour, IInteractable
{
    [SerializeField] private CaesarCipherPuzzleUIScript caesarCipherPuzzleUIScript;

    public void Interact()
    {
        if (caesarCipherPuzzleUIScript.GetState() == PuzzleUIState.Close)
        {
            caesarCipherPuzzleUIScript.Open();
        }
    }

    private void Update()
    {
        if (caesarCipherPuzzleUIScript.GetState() == PuzzleUIState.Open)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                caesarCipherPuzzleUIScript.Close();
            }
        }
    }
}
