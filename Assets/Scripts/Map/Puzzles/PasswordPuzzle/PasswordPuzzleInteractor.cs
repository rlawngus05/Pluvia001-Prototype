using UnityEngine;
using UnityEngine.UIElements;

public class PasswordPuzzleInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private UIDocument GUI;
    private VisualElement _root;
    
    private void Awake()
    {
        _root = GUI.rootVisualElement;
    }

    public void Interact()
    {
        if (_root.style.display == DisplayStyle.Flex) //* 조건식은 UI가 꺼져있는 상태인 경우을 의미함
        {
            CloseUI();
        }
        else
        {
            OpenUI();
        }
    }

    public void OpenUI() { _root.style.display = DisplayStyle.Flex; }

    public void CloseUI() { _root.style.display = DisplayStyle.None; }
}
