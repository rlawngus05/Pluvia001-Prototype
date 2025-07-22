using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;

    public Sprite GetIcon() { return itemData.Icon; }
    public string GetName() { return itemData.Name; }
    public string GetContent() { return itemData.Content; }

    public void Interact()
    {
        InventoryManager.Instance.InsertItem(itemData);

        Destroy(gameObject);
    }
}
