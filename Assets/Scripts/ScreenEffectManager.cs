using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScreenEffectManager : MonoBehaviour
{
    public static ScreenEffectManager Instance { get; private set; }

    [Header("Fade Effect Elements")]
    [SerializeField] private GameObject _fadePanel;
    private Coroutine _currentFadeCoroutine; //* 페이드 인/아웃 효과가 동시에 두개가 있게 하지 않기 위한 변수이다.

    [Header("Hurt Effect Elements")]
    [SerializeField] private ScriptableRendererFeature _hurtScreenEffect;
    [SerializeField] private Material _hurtScreenEffectMaterial;
    [SerializeField] private float _impulseTime;
    private Coroutine _currentHurtEffectCoroutine;

    [Header("Dead Effect Elements")]
    [SerializeField] private GameObject _deadEventPanel;
    [SerializeField] private ScriptableRendererFeature _deadScreenEffect;
    [SerializeField] private Material _deadScreenEffectMaterial;
    [SerializeField] private float _deadZoomTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        OffAllEffects();
    }

    private void OnDestroy()
    {
        OffAllEffects();
    }

    public void OffAllEffects()
    {
        _hurtScreenEffect.SetActive(false);
        
        _deadScreenEffect.SetActive(false);
        _deadEventPanel.SetActive(false);
    }

    //*                     
    //* 페이드 효과 관련 코드 
    //*

    //* 반환형이 Coroutine인 이유는 호출한 쪽에서 페이드인이 완전히 끝났을 때의 순간이 필요할 수 있기 때문이다.
    public Coroutine FadeIn(float duration = 1.0f)
    {
        if (_currentFadeCoroutine != null) StopCoroutine(_currentFadeCoroutine);
        _currentFadeCoroutine = StartCoroutine(FadeRoutine(0f, 1f, duration));

        return _currentFadeCoroutine;
    }

    //* 반환형이 Coroutine인 이유는 호출한 쪽에서 페이드 아웃이 완전히 끝났을 때의 순간이 필요할 수 있기 때문이다.
    public Coroutine FadeOut(float duration = 1.0f)
    {
        if (_currentFadeCoroutine != null) StopCoroutine(_currentFadeCoroutine);
        _currentFadeCoroutine = StartCoroutine(FadeRoutine(1f, 0f, duration));

        return _currentFadeCoroutine;
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        CanvasGroup canvasGroup = _fadePanel.GetComponent<CanvasGroup>();

        _fadePanel.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, EasingFunctions.EaseIn(t));
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (endAlpha == 0f)
            _fadePanel.SetActive(false);

        _currentFadeCoroutine = null;
    }

    //*                     
    //* 피격 효과 관련 코드
    //*
    
    public void ExecuteHurtEffect(float currentHealth, float maxHealth, float maxEffectStartHealth, float invincibleTime)
    {
        if (_currentHurtEffectCoroutine != null) { StopCoroutine(_currentHurtEffectCoroutine); }
        _currentHurtEffectCoroutine = StartCoroutine(HurtEffectCoroutine(currentHealth, maxHealth, maxEffectStartHealth, invincibleTime));
    }

    
    private IEnumerator HurtEffectCoroutine(float currentHealth, float maxHealth, float maxEffectStartHealth, float invincibleTime)
    {
        _hurtScreenEffectMaterial.SetFloat("_OverallAlpha", .0f);

        int coverSizePropertyIndex = _hurtScreenEffectMaterial.shader.FindPropertyIndex("_CoverSize");
        Vector2 coverSizeRange = _hurtScreenEffectMaterial.shader.GetPropertyRangeLimits(coverSizePropertyIndex);

        float coverSizeMin = coverSizeRange.x;
        float coverSizeMax = coverSizeRange.y;

        float coverSize = coverSizeMin + (coverSizeMax - coverSizeMin) * ((maxHealth - Mathf.Clamp(currentHealth, maxEffectStartHealth, maxHealth)) / (maxHealth - maxEffectStartHealth));
        _hurtScreenEffectMaterial.SetFloat("_CoverSize", coverSize);

        _hurtScreenEffect.SetActive(true);

        float elapsed = .0f;
        while (elapsed <= _impulseTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(.0f, 1.0f, EasingFunctions.FullyCubicIn(elapsed / _impulseTime));
            _hurtScreenEffectMaterial.SetFloat("_OverallAlpha", t);
            yield return null;
        }

        elapsed = .0f;
        while (elapsed <= invincibleTime - _impulseTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Lerp(1.0f, .0f, EasingFunctions.FullyCubicIn(elapsed / (invincibleTime - _impulseTime)));

            _hurtScreenEffectMaterial.SetFloat("_OverallAlpha", t);
            yield return null;
        }

        _hurtScreenEffect.SetActive(false);
    }

    //*                     
    //* 사망 효과 관련 코드
    //*

    public void ExecuteDeadEffect() { StartCoroutine(ExecuteDeadEffectCoroutine()); }

    public IEnumerator ExecuteDeadEffectCoroutine()
    {
        _deadScreenEffect.SetActive(true);

        float elapsed = .0f;

        while (elapsed <= _deadZoomTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / _deadZoomTime;
            _deadScreenEffectMaterial.SetFloat("_OverallAlpha", EasingFunctions.EaseOutCirc(t));

            yield return null;
        }

        _deadEventPanel.SetActive(true);
    }
}