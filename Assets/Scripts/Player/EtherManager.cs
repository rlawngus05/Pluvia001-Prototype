using System;
using UnityEngine;

public class EtherManager : MonoBehaviour
{
    public static EtherManager Instance { get; private set; }
    [SerializeField] private int _etherCount;
    public int EtherCount => _etherCount; 

    [SerializeField] private int _damage;
    [SerializeField] private float _damagingTime; //* 단위는 '초'임
    private float accumulatedTime;

    private Action<int> _etherCountObersver;
    public void SetEtherCountObserver (Action<int> evt) { _etherCountObersver = evt; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        accumulatedTime = 0;
    }

    public void AddEtherCount(int value = 1)
    {
        _etherCount += value;
        _etherCountObersver(_etherCount);
    }

    //! Test
    [ContextMenu(nameof(AddOneEther))]
    public void AddOneEther()
    {
        AddEtherCount(1);
    }

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
