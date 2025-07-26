using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private int _health;
    public int Health => _health;

    private const int MAX_HEALTH = 100;
    private const int MIN_HEALTH = 0;

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
        _health += value;
        if (_health > MAX_HEALTH)
        {
            _health = MAX_HEALTH;
        }

        _healthChangeObserver(_health, MAX_HEALTH);
    }

    public void DecreaseHealth(int value = 1)
    {
        _health -= value;
        if (_health < MIN_HEALTH)
        {
            _health = MIN_HEALTH;
        }

        _healthChangeObserver(_health, MAX_HEALTH);
    }
    
    //! Test
    // private void Update() {
    //     if (Input.GetKey(KeyCode.LeftArrow))
    //     {
    //         DecreaseHealth();
    //     }
    //     if (Input.GetKey(KeyCode.RightArrow))
    //     {
    //         IncreaseHealth();
    //     }
    // }
}
