using System.Linq;
using UnityEngine;

//* 상호작용 가능한 모든 물체는 InteractableObject를 구현해야 한다.
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private Material _interactableEffectMaterial;
    [SerializeField] private AudioClip _interactSoundEffect;

    private SpriteRenderer _spriteRenderer;
    private Material[] _originalMaterials;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Interact()
    {
        if (_interactSoundEffect != null) { SoundManager.Instance.PlaySoundEffect(_interactSoundEffect); }
    }

    public virtual void OnInteractable()
    {
        _originalMaterials = _spriteRenderer.materials;

        _spriteRenderer.materials = _originalMaterials.Append(_interactableEffectMaterial).ToArray();
    }

    public virtual void OffInteractable()
    {
        _spriteRenderer.materials = _originalMaterials;
    }
    
    public void SetInteractable() { gameObject.tag = "Interactable"; }
    public void UnsetInteractable() { gameObject.tag = "Untagged"; }
}   
