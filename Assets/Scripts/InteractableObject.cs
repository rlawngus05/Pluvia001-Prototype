using System.Linq;
using UnityEngine;

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
