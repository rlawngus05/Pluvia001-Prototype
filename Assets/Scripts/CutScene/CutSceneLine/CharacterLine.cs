using System;
using UnityEngine;

[Serializable]
public class CharacterLine : CutSceneLine
{
    [SerializeField] private ActorData _actor;
    [SerializeField] private EmotionType _emotion;
    [SerializeField] private string _content;

    public EmotionType Emotion => _emotion;
    public string Content => _content;
    public string ActorName => _actor.Name;
    public Sprite Portrait => _actor.GetPortrait(_emotion);
    public AudioClip ActorTypeSoundEffect => _actor.TypeSoundEffect;
}
