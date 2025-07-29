using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] Sprite _icon;
    [SerializeField] string _itemName;
    [SerializeField] string _content;
    [SerializeField] bool _isUsable;

    public Sprite Icon => _icon;
    public string Name => _itemName;
    public string Content => _content;
    public bool IsUsable => _isUsable;
}
