using System.Linq;
using UnityEngine;

//* 상호작용 가능한 모든 물체는 InteractableObject를 구현해야 한다.
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private Material _interactableEffectMaterial;

    private SpriteRenderer _spriteRenderer;
    private Material[] _originalMaterials;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public abstract void Interact();

    public virtual void OnInteractable()
    {
        _originalMaterials = _spriteRenderer.materials;

        _spriteRenderer.materials = _originalMaterials.Append(_interactableEffectMaterial).ToArray();
    }

    public virtual void OffInteractable()
    {
        _spriteRenderer.materials = _originalMaterials;
    }
    
}   
