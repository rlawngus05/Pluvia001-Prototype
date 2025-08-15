using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthViewrManager : MonoBehaviour
{
    [SerializeField] private Image _healthFillBar;
    [SerializeField] private List<Sprite> _healthFillBarImages;

    [SerializeField] private Image _healthBarBorder;
    [SerializeField] private List<Sprite> _healthBarBorderImages;

    [SerializeField] private GameObject _heartIcon;
    [SerializeField] private List<Sprite> _heartIconImages;

    [SerializeField] private GameObject _addictedHeartIcon;

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


    private void Start()
    {
        HealthManager.Instance.SetHealthChangeObserver((int currentHealth, int maxHealth) =>
        {
            float ratio = Mathf.Clamp01((float)currentHealth / maxHealth);

            _healthFillBar.fillAmount = ratio;
        });

        EtherManager.Instance.SetEtherCountObserver((int etherCount) =>
        {
            etherCount = Math.Clamp(etherCount, 0, 3);

            if (etherCount == 3)
            {
                _heartIcon.SetActive(false);
                _addictedHeartIcon.SetActive(true);

                _healthBarBorder.sprite = _healthBarBorderImages[1];
                _healthFillBar.sprite = _healthFillBarImages[1];

                return;
            }

            _addictedHeartIcon.SetActive(false);
            _heartIcon.SetActive(true);

            _healthBarBorder.sprite = _healthBarBorderImages[0];
            _healthFillBar.sprite = _healthFillBarImages[0];

            Image _heartIconImage = _heartIcon.GetComponent<Image>();
            _heartIconImage.sprite = _heartIconImages[etherCount];
        });
    }
}
