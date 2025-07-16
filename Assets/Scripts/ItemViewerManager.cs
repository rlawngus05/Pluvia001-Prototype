using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemViewerManager : MonoBehaviour
{
    public static ItemViewerManager Instance { get; private set; }

    [SerializeField] GameObject gui;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemContentText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Open(ItemData itemData)
    {
        itemIcon.sprite = itemData.Icon;
        itemNameText.text = itemData.Name;
        itemContentText.text = itemData.Content;

        gui.SetActive(true);
    }

    public void Close()
    {
        gui.SetActive(false);
    }
}
