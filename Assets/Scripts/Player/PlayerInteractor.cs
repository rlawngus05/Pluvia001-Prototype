using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;

public class PlayerInteractor : MonoBehaviour
{
    public static PlayerInteractor Instance { get; private set; }

    private GameObject _currentInteractableGameObject;
    private InteractableObject[] _interactableObjects;
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
            //* 만약 해당 물체에 PlayerInteractor가 여럿 있으면, Priority가 가장 큰 것 부터 실행한다.
            if (_interactableObjects != null && Input.GetKeyDown(KeyCode.F))
            {
                Array.Sort(_interactableObjects);
                Array.Reverse(_interactableObjects);

                foreach (InteractableObject interactableObject in _interactableObjects)
                {
                    interactableObject.Interact();
                }
            }
        }
    }

    //* 상호작용 범위 내에 있는 상호작용 가능 물체 중에서, 플레이어와 가장 가까운 물체가 무엇인지 업데이트 하는 함수이다.
    private void UpdateClosestInteractable()
    {
        float shortestDistance = float.MaxValue;
        GameObject closest = null;

        foreach (var col in _currentColliders)
        {
            if (col == null) continue;

            float distance = Vector2.Distance(transform.position, col.transform.position);

            if (distance < shortestDistance && col.GetComponent<InteractableObject>() != null)
            {
                InteractableObject[] temp = col.GetComponents<InteractableObject>();

                //* 만약 InteractableObject가 여러개 있다면, 모든 InteractableObject가 IsInteractable일 때, 상호작용 할 수 있음
                bool hasUninteractable = false;
                foreach (InteractableObject interactableObject in temp)
                {
                    if (!interactableObject.IsInteractable)
                    {
                        hasUninteractable = true;
                        break;
                    }
                }
                if (hasUninteractable) { continue; }

                shortestDistance = distance;
                closest = col.gameObject;
            }
        }

        if (closest != _currentInteractableGameObject)
        {
            _currentInteractableGameObject = closest;

            if (_interactableObjects != null)
            {
                foreach (InteractableObject interactableObject in _interactableObjects)
                {
                    interactableObject.OffInteractable();
                }
            }

            _interactableObjects = _currentInteractableGameObject.GetComponents<InteractableObject>();

            foreach (InteractableObject interactableObject in _interactableObjects)
            {
                interactableObject.OnInteractable();
            }
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

            if (collision.gameObject == _currentInteractableGameObject)
            {
                foreach (InteractableObject interactableObject in _interactableObjects)
                {
                    interactableObject.OffInteractable();
                }

                _currentInteractableGameObject = null;
                _interactableObjects = null;
            }
        }
    }
    
}

