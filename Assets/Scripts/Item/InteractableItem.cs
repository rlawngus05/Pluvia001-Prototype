using UnityEngine;

public class InteractableItem : Item, IInteractable
{
    public void Interact()
    {
        InventoryManager.Instance.InsertItem(itemData);

        Destroy(gameObject);
    }
}
