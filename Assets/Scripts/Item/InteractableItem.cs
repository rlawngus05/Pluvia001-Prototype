using UnityEngine;

public class InteractableItem : Item, IInteractable
{
    public void Interact()
    {
        ItemViewerManager.Instance.Open(itemData);
    }
}
