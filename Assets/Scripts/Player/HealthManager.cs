using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private int _health;

    private const int MAX_HEALTH = 100;
    private const int MIN_HEALTH = 0;

    private void Awake()
    {
        // Singleton 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴 방지
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    public int GetHealth() => _health;

    public void IncreaseHealth(int value = 1)
    {
        _health += value;
        if (_health > MAX_HEALTH)
        {
            _health = MAX_HEALTH;
        }
    }

    public void DecreaseHealth(int value = 1)
    {
        _health -= value;
        if (_health < MIN_HEALTH)
        {
            _health = MIN_HEALTH;
        }
    }

    // * 이 코드는 오직 테스트 용도로만 사용됨
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         DecreaseHealth(10);
    //     }
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         IncreaseHealth(10);
    //     }
    // }
}
