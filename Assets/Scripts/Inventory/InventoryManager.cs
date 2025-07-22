using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] Inventory _inventory;
    private Dictionary<ItemData, int> _dictifiedInventory;
    private Action<Dictionary<ItemData, int>> _inventoryGuiObserver;

    public Dictionary<ItemData, int> GetInventory() { return _dictifiedInventory; }
    public void SetInventoryGuiOberver(Action<Dictionary<ItemData, int>> evt){ _inventoryGuiObserver = evt; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _dictifiedInventory = _inventory.ToDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InsertItem(ItemData itemData, int amount = 1)
    {
        _inventory.Add(itemData, amount);
        UpdateDictifiedInventroy();
    }

    public bool DeleteItem(ItemData itemData, int amount = 1)
    {
        if (_inventory.Delete(itemData, amount))
        {
            UpdateDictifiedInventroy();
            return true;
        }

        return false;
    }

    public CheckItemResult HasItem(ItemData itemData)
    {
        if (_dictifiedInventory.ContainsKey(itemData) && _dictifiedInventory[itemData] != 0)
        {
            return CheckItemResult.Has;
        }

        return CheckItemResult.NotHave;
    }

    public CheckItemResult HasItemWithAmount(ItemData itemData, int amount)
    {
        if (HasItem(itemData) != CheckItemResult.NotHave)
        {
            if (_dictifiedInventory[itemData] == amount) { return CheckItemResult.Exact; }
            if (_dictifiedInventory[itemData] < amount) { return CheckItemResult.Lack; }
            if (_dictifiedInventory[itemData] > amount) { return CheckItemResult.Over; }
        }

        return CheckItemResult.NotHave;
    }

    [ContextMenu("Update DictifiedInventroy")]
    // TODO : 특정 시점에서 인벤토리 로직에서 갯수가 0인 아이템은 삭제하는 로직 넣기
    private void UpdateDictifiedInventroy()
    {
        _dictifiedInventory = _inventory.ToDictionary();

        if (_inventoryGuiObserver != null)
        {
            _inventoryGuiObserver(_dictifiedInventory);
        }
    }
    

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
                dict[item.ItemData] = item.Amount;
            }

            return dict;
        }

        public void Add(ItemData itemData, int amount)
        {
            foreach (InventoryItem inventoryItem in _inventoryItems)
            {
                if (inventoryItem.ItemData == itemData)
                {
                    inventoryItem.Add(amount);
                    return;
                }
            }

            InventoryItem newItem = new InventoryItem(itemData);
            newItem.Add(amount);

            _inventoryItems.Add(newItem);
        }

        public bool Delete(ItemData itemData, int amount)
        {
            foreach (InventoryItem inventoryItem in _inventoryItems)
            {
                if (inventoryItem.ItemData == itemData)
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
        [SerializeField] private int _amount;
        public ItemData ItemData => _itemData;
        public int Amount => _amount;

        public InventoryItem(ItemData itemData)
        {
            _itemData = itemData;
            _amount = 0;
        }

        public void Add(int amount)
        {
            _amount += amount;
        }
        public bool Delete(int amount)
        {
            if (_amount >= amount)
            {
                _amount -= amount;
                return true;
            }

            return false;
        }
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