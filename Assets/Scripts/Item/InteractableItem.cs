using UnityEngine;

public class InteractableItem : InteractableObject
{
    [SerializeField] private ItemData itemData;

    public Sprite GetIcon() { return itemData.Icon; }
    public string GetName() { return itemData.Name; }
    public string GetContent() { return itemData.Content; }

    public override void Interact()
    {
        base.Interact();
        
        InventoryManager.Instance.InsertItem(itemData);

        Destroy(gameObject);
    }
}
