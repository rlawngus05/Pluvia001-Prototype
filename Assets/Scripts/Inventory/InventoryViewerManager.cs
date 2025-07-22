using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewerManager : MonoBehaviour
{
    [SerializeField] private GameObject _Gui;
    private InventoryState _currentState;

    [SerializeField] private List<InventorySlot> _slots;
    [SerializeField] private int _rowCount;
    [SerializeField] private InventorySlot _currentSlot;
    [SerializeField] private GameObject _slotFocusImage; //TODO : SlotFocusingImage 적용하기 (현재는 선택되면 배경 색만 바꿈)
    private int _focusingSlotRow;
    private int _focusingSlotColumn;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemContextText;
    [SerializeField] private TextMeshProUGUI _keyNotifier;

    public static InventoryViewerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _currentState = InventoryState.Idle;

        DontDestroyOnLoad(gameObject);
    }

    public void Open()
    {
        InventoryManager.Instance.SetInventoryGuiOberver(Renew); //! InvetoryManager와의 초기화 순서 문제 때문에, 인벤토리를 킬 때마다, Obsever를 세팅함

        _Gui.SetActive(true);
        PlayerController.Instance.SetState(PlayerState.OpenInventory);
        PlayerInteractor.Instance.SetState(PlayerState.OpenInventory);

        Renew(InventoryManager.Instance.GetInventory());
        _focusingSlotRow = 0;
        _focusingSlotColumn = 0;
        ChangeFocusingSlot();
    }

    public void Close()
    {
        _Gui.SetActive(false);
        PlayerController.Instance.SetState(PlayerState.Idle);
        PlayerInteractor.Instance.SetState(PlayerState.Idle);
    }

    private void Update()
    {
        if (_currentState == InventoryState.Idle) 
        //* 이 코드는 혹같은 거임. 원래 의도는, 인벤토리 열었을 때 플레이어 못 움직이게 하는거였음. 
        //* 근데, 인벤토리 열였을 때, 움직이면 인벤토리 닫는 것으로 얘기가 나옴. 
        //* 그래서 기존의 인벤토리을 열면 플레이어를 봉쇄시키는 코드에서, 움직이면 그것이 해제 되게 만들게 해놂. 
        //* 근데 이 구조가 사실상 필요없음. ItemViewerManager 또한 이와 같은 구조가 있음
        {
            if (_Gui.activeSelf == true)
            {
                if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape))
                {
                    Close();
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    ItemData currentItemData = _currentSlot.GetItemData();

                    if (currentItemData != null)
                    {
                        if (currentItemData.IsUsable) //* 사용 가능한 아이템이면, 아이템 1회 소모함
                        {
                            InventoryManager.Instance.DeleteItem(currentItemData);
                            ChangeFocusingSlot();
                        }

                        if (currentItemData is ViewableItemData viewableItemData) //* 상세 보기 가능 아이템이면, 해당 아이템 상세 보기 엶
                        {
                            ItemViewerManager.Instance.Open(viewableItemData); //* ItemViewerManager와의 양방향 참조가 있음
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_focusingSlotRow > 0)
                    {
                        _focusingSlotRow--;
                        ChangeFocusingSlot();
                    }
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_focusingSlotRow < 3)
                    {
                        _focusingSlotRow++;
                        ChangeFocusingSlot();
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (_focusingSlotColumn > 0)
                    {
                        _focusingSlotColumn--;
                        ChangeFocusingSlot();
                    }
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (_focusingSlotColumn < 3)
                    {
                        _focusingSlotColumn++;
                        ChangeFocusingSlot();
                    }
                }

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                {
                    Close();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Open();
                }
            }
        }
    }

    //? ChangeFocusSlot()에 이동할 정보에 대한 매개변수를 입력하는게 더 바람직하기 않을까?
    public void ChangeFocusingSlot()
    {
        _currentSlot?.UnsetFocused();

        _currentSlot = _slots[_focusingSlotRow * _rowCount + _focusingSlotColumn];

        ItemData currentSlotItemData = _currentSlot.GetItemData();

        _itemIcon.sprite = currentSlotItemData?.Icon;
        _itemNameText.text = currentSlotItemData?.Name;
        _itemContextText.text = currentSlotItemData?.Content;

        _keyNotifier.gameObject.SetActive(true);
        if (currentSlotItemData != null)
        {
            if (currentSlotItemData.IsUsable)
            {
                _keyNotifier.text = "F키를 눌러서 아이템 사용하기";
            }
            else if (currentSlotItemData is ViewableItemData)
            {
                _keyNotifier.text = "F키를 눌러서 아이템 상세 정보 보기";
            }
            else
            {
                _keyNotifier.gameObject.SetActive(false);
            }
        }
        else
        {
            _keyNotifier.gameObject.SetActive(false);
        }

        _currentSlot.SetFocused();
    }

    public void Renew(Dictionary<ItemData, int> dict)
    {
        if (_Gui.activeSelf == true)
        {
            int index = 0;

            foreach (KeyValuePair<ItemData, int> item in dict)
            {
                if (item.Value != 0)
                {
                    _slots[index].SetItemData(item.Key);
                    _slots[index].SetAmount(item.Value);
                    index++;
                }
            }

            while (index < _slots.Count)
            {
                _slots[index++].SetDefault();
            }

            ChangeFocusingSlot();
        }
    }

    public void SetState(InventoryState inventoryState)
    {
        _currentState = inventoryState;
    }
}

public enum InventoryState
{
    Idle,
    OpenItemViewer
}
