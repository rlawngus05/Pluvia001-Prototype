using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthViewrManager : MonoBehaviour
{
    [SerializeField] private Image _helathFillBar;
    public static HealthViewrManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        HealthManager.Instance.SetHealthChangeObserver((int currentHealth, int maxHealth) =>
        {
            float ratio = Mathf.Clamp01((float)currentHealth / maxHealth);

            _helathFillBar.fillAmount = ratio;
        });
    }
}
