using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryViewerManager : MonoBehaviour
{
    [SerializeField] private GameObject _Gui;

    [SerializeField] private List<InventorySlot> _slots;
    [SerializeField] private InventorySlot _currentSlot;
    private int _focusingSlotRow;
    private int _focusingSlotColumn;
    [SerializeField] private GameObject _slotFocusImage;

    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemContextText;
    [SerializeField] private TextMeshProUGUI _keyNotifier;
    private InventoryState _currentState;

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
        _Gui.SetActive(true);
        InventoryManager.Instance.SetOnDictionaryChangeToGuiEvent(Renew); //TODO : 이거 리펙토링하기. 초기화 문제 때매 null 체크하는 부분이 좀 생김
        PlayerController.Instance.SetState(PlayerState.OpenInventory);
        PlayerInteractor.Instance.SetState(PlayerState.OpenInventory);

        Renew(InventoryManager.Instance.GetDict());
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

    //TODO : ItemViewerManager와의 양방향 참조 문제 해결하기
    private void Update()
    {
        if (_currentState == InventoryState.Idle)
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
                            ItemViewerManager.Instance.Open(viewableItemData);
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

    public void ChangeFocusingSlot()
    {
        if (_currentSlot != null) //TODO : 널 체킹 리펙토링 할 수 있는지 확인하기
        {
            _currentSlot.UnsetFocused();
        }

        _currentSlot = _slots[_focusingSlotRow * 4 + _focusingSlotColumn]; //! 가로 슬롯 수의 갯수가 4이여야 작동함

        ItemData currentSlotItemData = _currentSlot.GetItemData();

        //? 왜 됨
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

            while (index < 16) //! 총 슬롯의 갯수가 딱 16개여야 작동함
            {
                _slots[index++].SetDefault();
            }
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
