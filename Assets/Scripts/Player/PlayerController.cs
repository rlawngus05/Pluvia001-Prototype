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
            _rb.linearVelocityX = _moveSpeed * moveDirection;
        }
    }

    public void MoveCharacter(Transform destination)
    {
        transform.position = destination.position;
    }

    public void SetState(PlayerState playerState)
    {
        _currentState = playerState;
    }
}

public enum PlayerState
{
    Idle,
    OpenInventory,
    OpenPuzzle
}