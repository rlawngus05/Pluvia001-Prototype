using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemAmount;

    [SerializeField] private Image _border;
    [SerializeField] private Sprite _focusedBorderImage;
    [SerializeField] private Sprite _idleBorderImage;

    public void SetItemData(ItemData itemData)
    {
        Color currentIconImageColor = _itemIcon.color;
        currentIconImageColor.a = 1.0f;
        _itemIcon.color = currentIconImageColor;

        _itemData = itemData;

        _itemIcon.sprite = _itemData.Icon;
    }
    public ItemData GetItemData() { return _itemData; }

    public void SetDefault()
    {
        Color currentIconImageColor = _itemIcon.color;
        currentIconImageColor.a = .0f;
        _itemIcon.color = currentIconImageColor;

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
        _border.sprite = _focusedBorderImage;
    }

    public void UnsetFocused()
    {
        _border.sprite = _idleBorderImage;
    }
}
