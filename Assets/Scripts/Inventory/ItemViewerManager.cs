using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemViewerManager : MonoBehaviour
{
    public static ItemViewerManager Instance { get; private set; }

    [SerializeField] private GameObject _gui;
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemContentText;
    [SerializeField] private GameObject _detailContentContainer;
    private GameObject _detailContent; //* ItemViewer을 키고 끌 때마다, Prefab을 다시 생성하고 삭제함

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

    private void Update() {
        if (_gui.activeSelf == true)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                Close();
            }
        }
    }

    public void Open(ViewableItemData itemData)
    {
        InventoryViewerManager.Instance.SetState(InventoryState.OpenItemViewer);

        _itemIcon.sprite = itemData.Icon;
        _itemNameText.text = itemData.Name;
        _itemContentText.text = itemData.Content;

        _detailContent = Instantiate(itemData.DetailContent);
        _detailContent.transform.SetParent(_detailContentContainer.transform, false);

        _gui.SetActive(true);
    }

    public void Close()
    {
        _gui.SetActive(false);
        Destroy(_detailContent);
        InventoryViewerManager.Instance.SetState(InventoryState.Opened); //* InventoryViewerManager와의 양방향 참조가 있음
    }
}
