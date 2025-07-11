using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenEffectManager : MonoBehaviour
{
    public static ScreenEffectManager Instance { get; private set; }

    [SerializeField] private List<AnimationCurve> easingCurves;
    [SerializeField] private GameObject fadePanel;

    private Coroutine currentFadeCoroutine; //* 페이드 인/아웃 효과가 동시에 두개가 있게 하지 않기 위한 변수이다.

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

    //* 반환형이 Coroutine인 이유는 호출한 쪽에서 페이드인이 완전히 끝났을 때의 순간이 필요할 수 있기 때문이다.
    public Coroutine FadeIn(float duration = 1.0f, EasingType easingType = EasingType.EaseInOut)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeRoutine(0f, 1f, duration, easingType));

        return currentFadeCoroutine;
    }

    //* 반환형이 Coroutine인 이유는 호출한 쪽에서 페이드 아웃이 완전히 끝났을 때의 순간이 필요할 수 있기 때문이다.
    public Coroutine FadeOut(float duration = 1.0f, EasingType easingType = EasingType.EaseInOut)
    {
        if (currentFadeCoroutine != null) StopCoroutine(currentFadeCoroutine);
        currentFadeCoroutine = StartCoroutine(FadeRoutine(1f, 0f, duration, easingType));

        return currentFadeCoroutine;
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration, EasingType easingType)
    {
        float elapsed = 0f;
        CanvasGroup canvasGroup = fadePanel.GetComponent<CanvasGroup>();
        AnimationCurve easingCurve = easingCurves[(int)easingType];

        fadePanel.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float alpha = Mathf.Lerp(startAlpha, endAlpha, easingCurve.Evaluate(t));
            canvasGroup.alpha = alpha;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;

        if (endAlpha == 0f)
            fadePanel.SetActive(false);

        currentFadeCoroutine = null;
    }

    // * 이 코드는 테스트 용도의 코드임
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         FadeIn();
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         FadeOut();
    //     }
    // }
}

//* easingCurves 리스트에서 각각의 인덱스에 있는 animationCurve와 의미가 일대일대응임.
public enum EasingType
{
    EaseInOut,
    EaseIn,
    EaseOut
}
