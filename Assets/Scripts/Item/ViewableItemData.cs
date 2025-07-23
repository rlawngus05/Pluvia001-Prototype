using UnityEngine;

[CreateAssetMenu(fileName = "ViewableItemData", menuName = "Scriptable Objects/ViewableItemData")]
public class ViewableItemData : ItemData
{
    [SerializeField] private GameObject detailContent;

    public GameObject DetailContent => detailContent;
}
