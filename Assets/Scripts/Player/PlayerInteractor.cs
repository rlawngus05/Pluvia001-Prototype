using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    private InteractableObject _interactObject;
    private List<Collider2D> _currentColliders = new List<Collider2D>();
    private bool _isInteratable;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _isInteratable = true;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        PlayerStateManager.Instance.Subscribe((PlayerState currentState) =>
        {
            if ((currentState & PlayerState.Uninteractable) == PlayerState.Uninteractable) { _isInteratable = false; }
            else { _isInteratable = true; }
        });
    }

    private void Update()
    {
        if (_isInteratable)
        {
            UpdateClosestInteractable();

            //* 상호작용 키를 누르면, 플레이어와 가장 가까이 있는 상호작용 물체와 상호작용한다.
            if (_interactObject != null && Input.GetKeyDown(KeyCode.F))
            {
                _interactObject.Interact();
            }
        }
    }

    //* 상호작용 범위 내에 있는 상호작용 가능 물체 중에서, 플레이어와 가장 가까운 물체가 무엇인지 업데이트 하는 함수이다.
    private void UpdateClosestInteractable()
    {
        float shortestDistance = float.MaxValue;
        InteractableObject closest = null;

        foreach (var col in _currentColliders)
        {
            if (col == null) continue;

            float distance = Vector2.Distance(transform.position, col.transform.position);
            InteractableObject interactableObject = col.GetComponent<InteractableObject>();

            if (interactableObject.IsInteractable && distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = interactableObject;
            }
        }

        if (closest != _interactObject)
        {
            _interactObject?.OffInteractable();

            _interactObject = closest;

            _interactObject.OnInteractable();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (!_currentColliders.Contains(collision))
            {
                _currentColliders.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            _currentColliders.Remove(collision);

            if (collision.GetComponent<InteractableObject>() == _interactObject)
            {
                _interactObject.OffInteractable();
                _interactObject = null;
            }
        }
    }
    
}

