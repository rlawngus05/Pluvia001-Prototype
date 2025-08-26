using UnityEngine;

[CreateAssetMenu(fileName = "HealItemData", menuName = "Scriptable Objects/ItemData/HealItemData")]
public class HealItemData : UsableItemData
{
    [SerializeField] private int _healAmount;

    public HealItemData()
    {
        base.Subscribe(() =>
        {
            HealthManager.Instance.IncreaseHealth(_healAmount);
        });
    }
}
