using System;
using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private int _health;
    public int Health => _health;

    [SerializeField] private bool _isInvincible;
    [SerializeField] private float _invincibleTime;

    private const int MAX_HEALTH = 100;
    private const int MIN_HEALTH = 0;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _hurtSoundEffect;
    [SerializeField] private AudioClip _deadSoundEffect;

    private Action<int, int> _healthChangeObserver;
    public void SetHealthChangeObserver(Action<int, int> observer)
    {
        _healthChangeObserver = observer;

        _health = MAX_HEALTH;
        _healthChangeObserver(_health, MAX_HEALTH);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseHealth(int value = 1)
    {
        if (_health < MAX_HEALTH)
        {
            _health += value;
            if (_health > MAX_HEALTH)
            {
                _health = MAX_HEALTH;
            }

            _healthChangeObserver(_health, MAX_HEALTH);
        }
    }

    public void DecreaseHealth(int value = 1)
    {
        if (!_isInvincible && _health > MIN_HEALTH)
        {
            _health -= value;

            if (_health < MIN_HEALTH)
            {
                _health = MIN_HEALTH;
            }

            StartCoroutine(GetInvincible());

            _healthChangeObserver(_health, MAX_HEALTH);

            if (_health == MIN_HEALTH)
            {
                OnDead();
            }
            else
            {
                OnHurt();
            }
        }
    }

    private IEnumerator GetInvincible()
    {
        _isInvincible = true;

        yield return new WaitForSeconds(_invincibleTime);

        _isInvincible = false;
    }

    private void OnHurt()
    {
        SoundManager.Instance.PlaySoundEffectWithRandomPich(_hurtSoundEffect);
        CameraManager.Instance.ExectueHurtEffect(_health, MAX_HEALTH, 30);
        ScreenEffectManager.Instance.ExecuteHurtEffect(_health, MAX_HEALTH, 30, _invincibleTime);
    }

    private void OnDead()
    {
        SoundManager.Instance.PlaySoundEffect(_deadSoundEffect);

        PlayerStateManager.Instance.SetState(PlayerState.Uncontrolable);
        CameraManager.Instance.ChangeConfiner(null);
        Debug.Log("사망 애니매이션 실행");

        CameraManager.Instance.ExecuteDeadEffect();
        ScreenEffectManager.Instance.ExecuteDeadEffect();
    }

}
