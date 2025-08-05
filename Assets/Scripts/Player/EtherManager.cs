using UnityEngine;

public class EtherManager : MonoBehaviour
{
    public static EtherManager Instance { get; private set; }
    [SerializeField] private int _etherCount;
    public int EtherCount => _etherCount; 

    [SerializeField] private int _damage;
    [SerializeField] private float _damagingTime; //* 단위는 '초'임
    private float accumulatedTime; 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        accumulatedTime = 0;
    }

    public void AddEtherCount(int value = 1) { _etherCount += value; }

    void Update(){
        if (_etherCount >= 3)
        {
            if (accumulatedTime >= _damagingTime)
            {
                HealthManager.Instance.DecreaseHealth(_damage);

                accumulatedTime = 0;
            }
            else
            {
                accumulatedTime += Time.deltaTime;
            }
        }
    }
}
