using UnityEngine;
using System.Collections.Generic;

public class PlayerInteractor : MonoBehaviour
{
    private IInteractable interactObject;
    private List<Collider2D> currentColliders = new List<Collider2D>();

    private void Update()
    {
        UpdateClosestInteractable();

        if (interactObject != null && Input.GetKeyDown(KeyCode.F))
        {
            interactObject.Interact();
        }
    }

    private void UpdateClosestInteractable()
    {
        float shortestDistance = float.MaxValue;
        IInteractable closest = null;

        foreach (var col in currentColliders)
        {
            if (col == null) continue; // 파괴된 오브젝트 방지

            float distance = Vector2.Distance(transform.position, col.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closest = col.GetComponent<IInteractable>();
            }
        }

        interactObject = closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            if (!currentColliders.Contains(collision))
            {
                currentColliders.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            currentColliders.Remove(collision);

            if (collision.GetComponent<IInteractable>() == interactObject)
            {
                interactObject = null;
            }
        }
    }
}
