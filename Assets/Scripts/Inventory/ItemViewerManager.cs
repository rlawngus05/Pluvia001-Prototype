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
    private GameObject _detailContent;

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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }

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
        _detailContent.transform.SetParent(_detailContentContainer.transform, false); //? 두번째 인자의 정체 뭔지 모름 

        _gui.SetActive(true);
    }

    public void Close()
    {
        _gui.SetActive(false);
        Destroy(_detailContent);
        InventoryViewerManager.Instance.SetState(InventoryState.Idle);
    }
}
