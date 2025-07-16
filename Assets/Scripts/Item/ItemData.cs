using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] Sprite icon;
    [SerializeField] string itemName;
    [SerializeField] string content;

    public Sprite Icon { get { return icon; } }
    public string Name { get { return itemName; } }
    public string Content { get { return content; } }
}
