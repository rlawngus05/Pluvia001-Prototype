using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using System.Linq;

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

                foreach (InteractableObject interactableObject in _interactableObjects)
                {
                    interactableObject.Interact();
                }
            }
        }
    }

    /// <summary>
    /// 상호작용 범위 내의 상호작용 가능 오브젝트 중, 
    /// 플레이어와 가장 가까운 오브젝트를 갱신하는 함수
    /// </summary>
    //! 시발 이거 왜 되는지 몰라
    private void UpdateClosestInteractable()
    {
        float shortestDistance = float.MaxValue;
        GameObject closest = _currentInteractableGameObject;
        List<InteractableObject> temp = null;

        foreach (var col in _currentColliders)
        {
            if (col == null) continue;

            float distance = Vector2.Distance(transform.position, col.transform.position);
            var interactables = col.GetComponents<InteractableObject>();

            // 상호작용 가능한 객체가 하나라도 있는지 확인
            int interactableCount = interactables.Count(io => io.IsInteractable);

            if (interactableCount == 0)
            {
                // 이전까지 interactable이었는데 이제는 불가능한 경우
                if (col.gameObject == _currentInteractableGameObject)
                {
                    foreach (var io in _interactableObjects)
                        io.OffInteractable();

                    _currentInteractableGameObject = null;
                    _interactableObjects = null;
                }
                continue;
            }

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = col.gameObject;
                temp = interactables.Where(io => io.IsInteractable).ToList();
            }
        }

        // 가까운 오브젝트가 바뀐 경우
        if (closest != _currentInteractableGameObject && temp != null)
        {
            // 기존 이펙트 해제
            if (_interactableObjects != null)
            {
                foreach (var io in _interactableObjects)
                    io.OffInteractable();
            }

            _currentInteractableGameObject = closest;
            _interactableObjects = temp.ToArray();

            // 새 오브젝트 이펙트 켜기
            if (_interactableObjects != null)
            {
                foreach (var io in _interactableObjects)
                    io.OnInteractable();
            }
        }
        // 같은 오브젝트지만 interactable 구성 변동이 생긴 경우
        else if (temp != null && closest != null)
        {
            var newer = temp.Except(_interactableObjects).ToList();   // 새로 가능해진 것
            var deleter = _interactableObjects.Except(temp).ToList(); // 불가능해진 것

            foreach (var io in newer)
                io.OnInteractable();

            foreach (var io in deleter)
                io.OffInteractable();

            _interactableObjects = temp.ToArray();
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

