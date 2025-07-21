using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemAmount;
    [SerializeField] private Image _border;

    public void SetItemData(ItemData itemData)
    {
        _itemData = itemData;

        _itemIcon.sprite = _itemData.Icon;
    }
    public ItemData GetItemData() { return _itemData; }
    public void SetDefault()
    {
        _itemData = null;
        _itemIcon.sprite = null;
        _itemAmount.text = "";
    }
    public void SetAmount(int amount)
    {
        if (amount == 1)
        {
            _itemAmount.text = "";
        }
        else
        {
            _itemAmount.text = amount.ToString();
        }
    }

    public void SetFocused()
    {
        _border.color =  Color.red;
    }

    public void UnsetFocused()
    {
        _border.color =  Color.gray;
    }
}
