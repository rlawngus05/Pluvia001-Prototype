using UnityEngine;

public class EtherManager : MonoBehaviour
{
    //싱글톤 로직
    public static EtherManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private int etherCount;

    public void GetEther(int value = 1)
    {
        etherCount += value;
    }
}
