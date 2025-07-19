using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] protected ItemData originalItemData;
    protected ItemData itemData;

    private void Awake()
    {
        itemData = Instantiate(originalItemData);
    }

    public Sprite GetIcon() { return itemData.Icon; }
    public string GetName() { return itemData.Name; }
    public string GetContent() { return itemData.Content; }
}
