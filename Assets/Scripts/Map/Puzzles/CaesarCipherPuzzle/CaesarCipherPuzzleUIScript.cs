using UnityEngine;
using UnityEngine.UIElements;

public class CaesarCipherPuzzleUIScript : MonoBehaviour, IInitializableObject
{
    public void Init()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        root.style.display = DisplayStyle.None;
    }
}
