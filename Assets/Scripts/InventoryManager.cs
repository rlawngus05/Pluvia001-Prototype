using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] Inventory _inventory;
    private Dictionary<ItemData, int> _dictifyInventory;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _dictifyInventory = _inventory.ToDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InsertItem(ItemData item, int amount = 1)
    {
        _inventory.Add(item, amount);
        UpdateDictifyInventroy();
    }

    public bool Delete(ItemData item, int amount = 1)
    {
        if (_inventory.Delete(item, amount))
        {
            UpdateDictifyInventroy();
            return true;
        }

        return false;
    }

    public CheckItemResult HasItem(ItemData itemData)
    {
        if (_dictifyInventory.ContainsKey(itemData) && _dictifyInventory[itemData] != 0)
        {
            return CheckItemResult.Has;
        }

        return CheckItemResult.NotHave;
    }

    public CheckItemResult HasItemWithAmount(ItemData itemData, int amount)
    {
        if (HasItem(itemData) != CheckItemResult.NotHave)
        {
            if (_dictifyInventory[itemData] == amount) { return CheckItemResult.Exact; }
            if (_dictifyInventory[itemData] < amount) { return CheckItemResult.Lack; }
            if (_dictifyInventory[itemData] > amount) { return CheckItemResult.Over; }
        }

        return CheckItemResult.NotHave;
    }

    [ContextMenu("Update DictifyInventroy")]
    public void UpdateDictifyInventroy() { _dictifyInventory = _inventory.ToDictionary(); }
    

    //*
    //*
    //* 이 아래는 내부 클래스임
    //*
    //*

    [Serializable]
    private class Inventory
    {
        [SerializeField] private List<InventoryItem> _inventoryItems;

        public Dictionary<ItemData, int> ToDictionary()
        {
            Dictionary<ItemData, int> dict = new Dictionary<ItemData, int>();

            foreach (InventoryItem item in _inventoryItems)
            {
                dict[item.GetItemData] = item.GetCount;
            }

            return dict;
        }

        public void Add(ItemData item, int amount = 1)
        {
            foreach (InventoryItem inventoryItem in _inventoryItems)
            {
                if (inventoryItem.GetItemData == item)
                {
                    inventoryItem.Add(amount);
                    return;
                }
            }

            InventoryItem newItem = new InventoryItem(item);
            newItem.Add(amount);

            _inventoryItems.Add(newItem);
        }

            public bool Delete(ItemData item, int amount = 1)
        {
            foreach (InventoryItem inventoryItem in _inventoryItems)
            {
                if (inventoryItem.GetItemData == item)
                {
                    return inventoryItem.Delete(amount);
                }
            }

            return false;
        }
    }

    [Serializable]
    private class InventoryItem
    {
        [SerializeField] private ItemData _itemData;
        [SerializeField] private int _count;

        public InventoryItem(ItemData itemData)
        {
            _itemData = itemData;
            _count = 0;
        }

        public ItemData GetItemData => _itemData;
        public int GetCount => _count;

        public void Add(int value = 1)
        {
            _count += value;
        }
        public bool Delete(int value = 1)
        {
            if (_count > value)
            {
                _count -= value;
                return true;
            }

            return false;
        }
    }

    //! Test
    [SerializeField] ItemData testItem;
    [SerializeField] int testCount;

    [ContextMenu("HasItem() Test")]
    public void HasItemTest()
    {
        Debug.Log(HasItemWithAmount(testItem, testCount));
    }
}

public enum CheckItemResult
{
    Has,
    NotHave,
    Exact,
    Lack,
    Over
}