using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData/DefaultItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] protected Sprite _icon;
    [SerializeField] protected string _itemName;
    [SerializeField] protected string _content;

    public Sprite Icon => _icon;
    public string Name => _itemName;
    public string Content => _content;
}
