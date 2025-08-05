using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _earlyJumpDivder;
    [SerializeField] private float _unholdJumpGravityScale;
    [SerializeField] private float _maxFallVelocity;
    [SerializeField] private Animator _animator;
    private float _originGravityScale;
    private bool _isJump;

    private Rigidbody2D _rb;
    private SpriteRenderer _spr;
    [SerializeField] private PlayerState _currentState;

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
        _spr = GetComponent<SpriteRenderer>();
        _currentState = PlayerState.Idle;
        _isJump = false;
        _originGravityScale = 1.0f;
    }

    private void FixedUpdate()
    {
        //* 좌우 이동 로직
        if (_currentState == PlayerState.Idle)
        {
            float moveDirection = Input.GetAxisRaw("Horizontal");
            SetVelocityX(moveDirection, _moveSpeed);
            //* 걷는 애니메이션 + 좌우 전환 로직
            if (moveDirection == 0.0f)
            {
                _animator.SetBool("isWalking", false);
            }
            else
            {   
                _animator.SetBool("isWalking", true);
                if (moveDirection == 1.0f) { _spr.flipX = true; }
                if (moveDirection == -1.0f) { _spr.flipX = false; }
            }
            
        }

        _rb.linearVelocityY = Mathf.Max(_rb.linearVelocityY, -_maxFallVelocity); //* 낙하 속도 최대치 설정
        //* 점프 애니메이션 실행
        if (_isJump) { _animator.SetBool("isJumping", _isJump); }
        if (_rb.linearVelocityY < 0 && _isJump) { _animator.SetBool("isFalling", true); }
        if (!_isJump)
        {
            _animator.SetBool("isJumping", _isJump);
            _animator.SetBool("isFalling", false);
        }
        
    }

    private void Update()
    {
        if (_currentState == PlayerState.Idle)
        {
            //* 점프 관련 로직
            if (Input.GetKeyDown(KeyCode.Space) && !_isJump)
            {
                _rb.AddForceY(_jumpForce, ForceMode2D.Impulse);

                _isJump = true;
            }

            //* 점프키 놓았을 때, 중력 크기 키움
            if (Input.GetKeyUp(KeyCode.Space) && _isJump)
            {
                //* 일찍 때었을 경우, 상승 속도를 줄임
                if (_rb.linearVelocityY > 0) { _rb.linearVelocityY /= _earlyJumpDivder; }

                //* 점프 키를 땠을 때, 중력을 늘림
                if (_rb.gravityScale != _unholdJumpGravityScale) //* 점프키 여러번 눌렀을 때, gravityScale이 _jumpKeyUnholdGravityScaler로 고정 되는 문제를 해결하는 조건
                {
                    _originGravityScale = _rb.gravityScale;
                    _rb.gravityScale = _unholdJumpGravityScale;
                }
            }

            //* 인벤토리 열기 로직
            if (Input.GetAxisRaw("Horizontal") == 0.0f && !_isJump)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    InventoryViewerManager.Instance.Open();

                    _currentState = PlayerState.OpenInventory;
                    PlayerInteractor.Instance.SetState(PlayerState.OpenInventory);
                }
            }
            
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isJump && collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _isJump = false;
            _rb.gravityScale = _originGravityScale;
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
    MoveArea,
    Dead
}