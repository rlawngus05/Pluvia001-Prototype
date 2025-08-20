using System.Linq;
using UnityEngine;

//* 상호작용 가능한 모든 물체는 InteractableObject를 구현해야 한다.
public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] private Material _interactableEffectMaterial;
    [SerializeField] private AudioClip _interactSoundEffect;
    [SerializeField] private bool _isInteractable;
    public bool IsInteractable => _isInteractable;

    private SpriteRenderer _spriteRenderer;
    private Material[] _originalMaterials;

    protected virtual void Awake()
    {
        _isInteractable = true;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact()
    {
        if (!_isInteractable) { return; }
        if (_interactSoundEffect != null) { SoundManager.Instance.PlaySoundEffect(_interactSoundEffect); }
        OnInteract();
    }

    protected abstract void OnInteract();

    public virtual void OnInteractable()
    {
        _originalMaterials = _spriteRenderer.materials;

        _spriteRenderer.materials = _originalMaterials.Append(_interactableEffectMaterial).ToArray();
    }

    public virtual void OffInteractable()
    {
        _spriteRenderer.materials = _originalMaterials;
    }

    public void SetInteractable() { _isInteractable = true; }
    public void UnsetInteractable() { _isInteractable = false; }
}   
