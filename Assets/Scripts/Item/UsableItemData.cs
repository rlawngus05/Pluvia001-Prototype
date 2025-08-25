using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UsableItemData", menuName = "Scriptable Objects/ItemData/UsableItemData")]
public class UsableItemData : ItemData
{
    [SerializeField] private AudioClip _usedSoundEffect;
    private event Action _onUsed;

    public void Subscribe(Action evt) { _onUsed += evt; }
    public void Unsubscribe(Action evt) { _onUsed -= evt; }
    public void Use()
    {
        SoundManager.Instance.PlaySoundEffect(_usedSoundEffect);
        _onUsed?.Invoke();
    }
}
