using UnityEngine;
using UnityEngine.UIElements;

public class PasswordPuzzleUIController : MonoBehaviour, IInteractable
{
    [SerializeField] private UIDocument GUI;
    [SerializeField] private PasswordPuzzleUIScript passwordPuzzleUIScript;
    private VisualElement _root;
    
    private void Awake()
    {
        _root = GUI.rootVisualElement;
    }

    public void Interact()
    {
        if (passwordPuzzleUIScript.GetState() == PuzzleUIState.Close)
        {
            Open();
        }
    }

    private void Update()
    {
        if (passwordPuzzleUIScript.GetState() == PuzzleUIState.Open)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
        }
    }

    private void Open()
    {
        _root.style.display = DisplayStyle.Flex;

        PlayerController.Instance.SetState(PlayerState.OpenPuzzle);
        passwordPuzzleUIScript.SetState(PuzzleUIState.Open);
    }

    private void Close()
    {
        _root.style.display = DisplayStyle.None;

        PlayerController.Instance.SetState(PlayerState.Idle);
        passwordPuzzleUIScript.SetState(PuzzleUIState.Close);
    }
}
