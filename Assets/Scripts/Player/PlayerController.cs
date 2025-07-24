using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float _moveSpeed;
    private Rigidbody2D _rb;
    public PlayerState _currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        _rb = GetComponent<Rigidbody2D>();
        _currentState = PlayerState.Idle;

    }

    private void FixedUpdate()
    {
        if (_currentState == PlayerState.Idle)
        {
            float moveDirection = Input.GetAxisRaw("Horizontal");
            SetVelocityX(moveDirection, _moveSpeed);
        }
    }

    private void Update()
    {
        if (_currentState == PlayerState.Idle && Input.GetAxisRaw("Horizontal") == 0.0f)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InventoryViewerManager.Instance.Open();

                _currentState = PlayerState.OpenInventory;
                PlayerInteractor.Instance.SetState(PlayerState.OpenInventory);
            }
        }
    }

    private void SetVelocityX(float moveDirection, float moveSpeed) { _rb.linearVelocityX = _moveSpeed * moveDirection; }

    public void MoveCharacter(Transform destination)
    {
        transform.position = destination.position;
    }

    public void SetState(PlayerState playerState)
    {
        StartCoroutine(SetStateCoroutine(playerState));
    }

    //* 변경된 상태가, 현재 프레임 부터 적용되지 않고 다음 프레임 부터 적용되도록 하는 보조함수
    private IEnumerator SetStateCoroutine(PlayerState playerState)
    {
        yield return null;

        _currentState = playerState;
        PlayerInteractor.Instance.SetState(playerState);
        
        if (playerState == PlayerState.MoveArea) { SetVelocityX(0.0f, 0.0f); }
    }
}

public enum PlayerState
{
    Idle,
    OpenInventory,
    OpenPuzzle,
    MoveArea
}