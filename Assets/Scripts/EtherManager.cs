using UnityEngine;

public class EtherManager : MonoBehaviour
{
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

    public void AddEtherCount(int value = 1)
    {
        etherCount += value;
    }

    public int GetEtherCount() { return etherCount; }

    void Update(){
        // TODO : 에테르의 갯수에 비례하여 캐릭터 채력 지속적으로 감소 시키기
    }
}
