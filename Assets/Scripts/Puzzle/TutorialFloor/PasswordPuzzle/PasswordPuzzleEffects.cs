using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPuzzleEffects : MonoBehaviour
{
    private Vector2 originalPos;

    [Header("Slot Scroll/Coloring Effect")]
    [SerializeField] private List<GameObject> _slotScrolls;
    [SerializeField] private float _slotHeight;
    [SerializeField] private float _slotScrollTime;

    [Header("Shake Effect")]
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] private float _shakePower;
    [SerializeField] private float _shakeTime;
    [SerializeField] private float _shakeAmplitude;
    private Coroutine _currentShakeEffectCoroutine;

    [Header("Magic Circle Effect")]
    [SerializeField] private GameObject _magicCircle;
    [SerializeField] private GameObject _star;
    [SerializeField] private GameObject _circle0;
    [SerializeField] private GameObject _circle1;
    [SerializeField] private float _spinSpeed;
    [SerializeField] private float _accelerateTime;
    [SerializeField] private float _spinTime;
    [SerializeField] private float _decelerateTime;

    [Header("Ether Container Effect")]
    [SerializeField] private GameObject _etherContainer;
    [SerializeField] private float _etherContainerHeight;
    [SerializeField] private float _elevateTime;
    [SerializeField] private InteractableEther _interactableEther;
    [SerializeField] private float _etherShowTime;
    [SerializeField] private AudioClip _elevateSoundEffect;

    [Header("Fail Effect")]
    [SerializeField] private float _pushedPowerX;

    private void Awake()
    {
        originalPos = _rectTransform.position;
    }

    public Coroutine SlotNumberScrollingAnimation(int prev, int cur, int index) { return StartCoroutine(SlotNumberScrollingAnimationCoroutine(prev, cur, index)); }
    private IEnumerator SlotNumberScrollingAnimationCoroutine(int prev, int cur, int index)
    {
        RectTransform rectTransform = _slotScrolls[index].GetComponent<RectTransform>();

        float from = -(-_slotHeight * prev);
        float to = -(-_slotHeight * cur);

        float elapsed = .0f;

        while (elapsed <= _slotScrollTime)
        {
            elapsed += Time.deltaTime;

            float curOffsetMax = Mathf.Lerp(from, to, EasingFunctions.EaseOutQuint(elapsed / _slotScrollTime));

            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, curOffsetMax);

            yield return null;
        }
    }

    public void SetScrollColor(int index, Color32 changedColor)
    {
        GameObject slotScroll = _slotScrolls[index];
        for (int i = 0; i < slotScroll.transform.childCount; i++)
        {
            Image numberImage = slotScroll.transform.GetChild(i).GetChild(0).GetComponent<Image>();
            numberImage.color = changedColor;
        }
    }

    public void ShakeEffect()
    {
        if (_currentShakeEffectCoroutine != null)
        {
            _rectTransform.position = originalPos;
            StopCoroutine(_currentShakeEffectCoroutine);
        }
        _currentShakeEffectCoroutine = StartCoroutine(ShakeEffectCorutine());
    }
    public IEnumerator ShakeEffectCorutine()
    {
        originalPos = _rectTransform.position;

        float elapsed = .0f;

        while (elapsed <= _shakeTime)
        {
            elapsed += Time.deltaTime;

            Vector2 shakingPos = new Vector2(UnityEngine.Random.Range(.0f, 1.0f), UnityEngine.Random.Range(.0f, 1.0f)) * _shakePower;

            _rectTransform.position = originalPos + shakingPos;

            yield return null;
        }

        _rectTransform.position = originalPos;
    }

    public Coroutine SpinMagicCircle() { return StartCoroutine(SpinMagicCircleCoroutine()); }
    private IEnumerator SpinMagicCircleCoroutine()
    {
        RectTransform rectTransformStar = _star.GetComponent<RectTransform>();
        RectTransform rectTransformCircle0 = _circle0.GetComponent<RectTransform>();
        RectTransform rectTransformCircle1 = _circle1.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = _magicCircle.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        _magicCircle.SetActive(true);

        float elapsed = 0f;
        float currentAngularVelocity = 0f; // Track speed over time for smoother acceleration

        // == Phase 1: Acceleration & Fade-In ==
        while (elapsed < _accelerateTime)
        {
            float t = elapsed / _accelerateTime;

            // Fade in (using the original bounce for style)
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, EasingFunctions.EaseInOutBounce(t));

            // Smoothly increase rotation speed from 0 to _spinSpeed
            currentAngularVelocity = Mathf.Lerp(0f, _spinSpeed, EasingFunctions.EaseInQuint(t));

            // Apply rotation based on the current speed
            rectTransformStar.Rotate(0.0f, 0.0f, currentAngularVelocity * Time.deltaTime);
            rectTransformCircle0.Rotate(0.0f, 0.0f, -currentAngularVelocity * Time.deltaTime);
            rectTransformCircle1.Rotate(0.0f, 0.0f, currentAngularVelocity * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure values are set correctly for the next phase
        canvasGroup.alpha = 1.0f;
        elapsed = 0f;

        // == Phase 2: Constant Spin (Corrected) ==
        while (elapsed < _spinTime)
        {
            // Rotate by the constant max speed each frame
            rectTransformStar.Rotate(0.0f, 0.0f, _spinSpeed * Time.deltaTime);
            rectTransformCircle0.Rotate(0.0f, 0.0f, -_spinSpeed * Time.deltaTime);
            rectTransformCircle1.Rotate(0.0f, 0.0f, _spinSpeed * Time.deltaTime);

            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // == Phase 3: Deceleration & Fade-Out (Corrected) ==
        while (elapsed < _decelerateTime)
        {
            // Use the correct duration for normalization
            float t = elapsed / _decelerateTime;

            // Decrease speed from _spinSpeed to 0
            currentAngularVelocity = Mathf.Lerp(_spinSpeed, 0f, EasingFunctions.EaseInCubic(t));

            // Apply the decreasing rotation
            rectTransformStar.Rotate(0.0f, 0.0f, currentAngularVelocity * Time.deltaTime);
            rectTransformCircle0.Rotate(0.0f, 0.0f, -currentAngularVelocity * Time.deltaTime);
            rectTransformCircle1.Rotate(0.0f, 0.0f, currentAngularVelocity * Time.deltaTime);

            // Fade out smoothly
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Hide the object after the animation is complete
        _magicCircle.SetActive(false);
    }


    public Coroutine ElevateEtherContainer() { return StartCoroutine(ElevateEtherContainerCoroutine()); }
    private IEnumerator ElevateEtherContainerCoroutine()
    {
        Transform etherContainerTransform = _etherContainer.transform;
        Vector3 initalPosition = etherContainerTransform.position;

        float elapsed = .0f;

        CameraManager.Instance.ShakeCamera(_shakeAmplitude);
        SoundManager.Instance.PlaySoundEffect(_elevateSoundEffect);

        while (elapsed < _elevateTime)
        {
            float easedT = EasingFunctions.EaseOutExpo(elapsed / _elevateTime);
            Vector3 currentPosition = initalPosition + new Vector3(0, _etherContainerHeight * easedT, 0);

            etherContainerTransform.position = currentPosition;

            yield return null;
            elapsed += Time.deltaTime;
        }

        CameraManager.Instance.StopShakeCamera();
    }

    public Coroutine ShowEther() { return StartCoroutine(ShowEtherCoroutine()); }
    private IEnumerator ShowEtherCoroutine()
    {
        SpriteRenderer spriteRenderer = _interactableEther.GetComponent<SpriteRenderer>();

        float elapsed = .0f;
        Color currentColor = spriteRenderer.color;

        while (elapsed <= _etherShowTime)
        {
            float alpha = Mathf.Lerp(.0f, 1.0f, EasingFunctions.EaseInCirc(elapsed / _etherShowTime));

            currentColor.a = alpha;
            spriteRenderer.color = currentColor;

            yield return null;
            elapsed += Time.deltaTime;
        }

        currentColor.a = 1.0f;
        spriteRenderer.color = currentColor;
    }

    public void PlayFailSequence()
    {
        Vector2 knockbackForce = new Vector2(-_pushedPowerX, 0);
        
        PlayerController.Instance.AddForce(knockbackForce, ForceMode2D.Impulse);

        Debug.Log("Hit animation executing"); // 피격 애니매이션 실행
    }
}
