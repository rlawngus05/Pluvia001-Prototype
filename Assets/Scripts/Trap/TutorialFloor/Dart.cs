using System;
using System.Collections;
using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] private float _velocity;
    [SerializeField] private float _startTime;
    [SerializeField] private AnimationCurve _startVelocityCurve;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;
    private Action _playerHitEvent;
    private bool _isFlip;
    private GameObject _dartDestroyer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _isFlip = false;
    }

    public void StartMove(Action evt, bool isFlip, GameObject dartDetroyer)
    {
        _playerHitEvent = evt;
        _isFlip = isFlip;
        _dartDestroyer = dartDetroyer;

        if (_isFlip) { _spriteRenderer.flipX = true; }
        StartCoroutine(MoveCoroutine());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerHitEvent();
            Destroy(gameObject);
        }

        if (collision.gameObject == _dartDestroyer) { Destroy(gameObject); }
    }

    private IEnumerator MoveCoroutine()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= _startTime)
        {
            elapsedTime += Time.deltaTime;

            float t = _startVelocityCurve.Evaluate(elapsedTime / _startTime);

            _rb.linearVelocityX = _isFlip ? _velocity * t * -1 : _velocity * t;

            yield return null;
        }

        _rb.linearVelocityX = _isFlip ? _velocity * -1 : _velocity;
    }

}
