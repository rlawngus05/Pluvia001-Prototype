using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class InteractableEther : InteractableItem
{
    [SerializeField] private float _floatTime;
    [SerializeField] private float _maxFloatingHeight;
    private float _initialY;
    private Coroutine _currentCoroutine;

    override protected void Awake()
    {
        base.Awake();
        ExecuteFloating();
    }

    protected override void OnInteract()
    {
        base.OnInteract();
        EtherManager.Instance.AddEtherCount();
    }

    public void ExecuteFloating()
    {
        _initialY = transform.localPosition.y;
        _currentCoroutine = StartCoroutine(Floating());
    }

    public void StopFloating()
    {
        if (_currentCoroutine != null) { StopCoroutine(_currentCoroutine); }
        transform.localPosition = new Vector3(transform.localPosition.x, _initialY, transform.localPosition.z);
    }

    private IEnumerator Floating()
    {
        float elapsed = .0f;

        while (elapsed <= _floatTime / 2)
        {
            float currentOffset  = Mathf.Lerp(.0f, _maxFloatingHeight, EasingFunctions.EaseOutQuad(elapsed / (_floatTime / 2)));
            Vector3 currentPos = new Vector3(transform.localPosition.x, _initialY + currentOffset , transform.localPosition.z);

            transform.localPosition = currentPos;

            yield return null;
            elapsed += Time.deltaTime;
        }

        elapsed = .0f;

        while (elapsed <= _floatTime / 2)
        {
            float currentOffset  = Mathf.Lerp(_maxFloatingHeight, .0f, EasingFunctions.EaseInQuad(elapsed / (_floatTime / 2)));
            Vector3 currentPos = new Vector3(transform.localPosition.x, _initialY + currentOffset , transform.localPosition.z);

            transform.localPosition = currentPos;

            yield return null;
            elapsed += Time.deltaTime;
        }

        _currentCoroutine = StartCoroutine(Floating());
    }
}
